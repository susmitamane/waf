﻿using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using UITest.Writer.Views;
using Xunit;
using Xunit.Abstractions;

namespace UITest.Writer;

public class WriterTest(ITestOutputHelper log) : UITest(log)
{
    [Fact]
    public void AboutTest()
    {
        Launch();
        var window = GetShellWindow();

        window.AboutButton.Click();

        var messageBox = window.FirstModalWindow().As<MessageBox>();
        Assert.Equal("Waf Writer", messageBox.Title);
        Log.WriteLine(messageBox.Message);
        Assert.StartsWith("Waf Writer ", messageBox.Message);
        messageBox.OkButton.Click();

        var fileRibbonMenu = window.FileRibbonMenu;
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.ExitMenuItem.Invoke();
    }

    [Fact]
    public void NewZoomWritePrintPreviewExitWithoutSave()
    {
        Launch();
        var window = GetShellWindow(maximize: true);

        var viewTab = window.ViewTab;
        viewTab.Select();
        var zoomInButton = viewTab.ZoomInButton;
        Assert.False(zoomInButton.IsEnabled);

        var startView = window.StartView;
        Assert.False(startView.IsOffscreen);
        startView.NewButton.Click();

        Retry.WhileTrue(() => startView.IsOffscreen);
        Assert.True(zoomInButton.IsEnabled);
        zoomInButton.Click();
        var zoomComboBox = window.ZoomComboBox;
        Assert.Equal("110%", zoomComboBox.EditableText);

        var richTextView = window.DocumentTabItems.Single().RichTextView;
        richTextView.RichTextBox.Text = "Hello World";

        var fileRibbonMenu = window.FileRibbonMenu;
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.PrintPreviewMenuItem.Invoke();
        
        var printPreviewTab = window.PrintPreviewTab;
        Assert.True(printPreviewTab.IsSelected);
        printPreviewTab.ZoomOutButton.Click();
        Assert.Equal("90%", zoomComboBox.EditableText);
        printPreviewTab.ClosePrintPreviewButton.Click();

        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.NewMenuItem.Invoke();

        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.ExitMenuItem.Invoke();

        var saveChangesWindow = window.FirstModalWindow().As<SaveChangesWindow>();
        var firstItem = saveChangesWindow.FilesToSaveList.Items.Single();
        Assert.Equal("Document 1.rtf", firstItem.Text);
        saveChangesWindow.NoButton.Click();
    }

    [Fact]
    public void NewSaveTodo()
    {
        Launch();
        var window = GetShellWindow();

        var startView = window.StartView;
        Assert.False(startView.IsOffscreen);
        startView.NewButton.Click();

        var tab1 = window.DocumentTabItems.Single();
        tab1.RichTextView.RichTextBox.Text = "Hello World";

        var fileRibbonMenu = window.FileRibbonMenu;
        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.NewMenuItem.Invoke();

        var tab2 = window.DocumentTabItems[1];
        Assert.Equal("Document 2.rtf", tab2.TabName);
        tab2.CloseButton.Invoke();

        Assert.True(tab1.IsSelected);
        tab1.CloseButton.Invoke();

        var saveChangesWindow = window.FirstModalWindow().As<SaveChangesWindow>();
        var firstItem = saveChangesWindow.FilesToSaveList.Items.Single();
        Assert.Equal("Document 1.rtf", firstItem.Text);
        saveChangesWindow.NoButton.Click();

        fileRibbonMenu.MenuButton.Click();
        fileRibbonMenu.ExitMenuItem.Invoke();
    }
}
