using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public string CurrentDate => String.Format("{0}/{1}/{2}",DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                selectedMonth = _selectedDate.Month.ToString();
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        public string selectedMonth;
        public string SelectedMonth
        {
            get
            {
                return selectedMonth;
            }
            set
            {
                selectedMonth = value;
                OnPropertyChanged(nameof(SelectedMonth));
            }
        }
        public DashboardViewModel(DateTime selectedDate)
        {
            _selectedDate = selectedDate;
        }
    }
}
