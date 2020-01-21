﻿using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.BookLibrary.Reporting.Applications.Views;

namespace Waf.BookLibrary.Reporting.Applications.ViewModels
{
    [Export]
    public class ReportViewModel : ViewModel<IReportView>
    {
        private object? report;

        [ImportingConstructor]
        public ReportViewModel(IReportView view) : base(view)
        {
        }

        public object? Report
        {
            get => report;
            set => SetProperty(ref report, value);
        }

        public ICommand? CreateBookListReportCommand { get; set; }
        
        public ICommand? CreateBorrowedBooksReportCommand { get; set; }
    }
}
