using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CDB_TSB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool resetCurrentPosition = true, setStartDate = false, setEndDate = false;
        private string currentCategory = "";
        private int listIndex = 0, currentPosition = 0;
        ListBox resultsBoxContent = new ListBox();
        ManageData CurrentData = new ManageData();
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(ReturnPressListener);

        }
        private void ReturnPressListener(object sender, KeyEventArgs key)
        {
            if (key.Key.ToString() == "Return")
            {
                SearchButton_Click(sender,key);
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;
            picker.DisplayDate = date.Value;
            if (picker.Name.Contains("Start"))
            {
                setStartDate = true;
            }
            if (picker.Name.Contains("End"))
            {
                setEndDate = true;
            }
        }

        public void AddResultsToGrid()
        {
                Results_Grid.Children.Add(resultsBoxContent);
            for (int i = 0; i < CurrentData.categoryCount.Count(); i++)
            {
                for (int j = 0; j < CurrentData.callLogData.Count; j++ ) {
                    if(CurrentData.categoryCount[i].Item1 == CurrentData.callLogData[j].Item3 && j > listIndex)
                    {
                        listIndex = j;
                    }
                }
                    _createExpander(CurrentData.categoryCount[i].Item1 + " : " + CurrentData.categoryCount[i].Item2, listIndex, i);
                listIndex = 0;
            }
        }

        private void _createExpander(string name, int listPosition, int dynamicIndexPosition)
        {
            Expander dynamicExpander = new Expander();
            dynamicExpander.Header = name;
            dynamicExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
            dynamicExpander.IsExpanded = false;
            dynamicExpander.Content = _createTextbox(listPosition, dynamicIndexPosition);

            resultsBoxContent.Items.Add(dynamicExpander);    
        }

        private Grid _createTextbox(int listPosition, int dynamicIndexPosition)
        {
            Grid logLayout = new Grid();
            RowDefinition grdTicketButtons = new RowDefinition();
            RowDefinition grdTicketContent = new RowDefinition();
            RowDefinition grdLogButtons = new RowDefinition();
            RowDefinition grdEmptyRow1 = new RowDefinition();
            RowDefinition grdEmptyRow2 = new RowDefinition();
            logLayout.RowDefinitions.Add(grdTicketButtons);
            logLayout.RowDefinitions.Add(grdEmptyRow1);
            logLayout.RowDefinitions.Add(grdTicketContent);
            logLayout.RowDefinitions.Add(grdEmptyRow2);
            logLayout.RowDefinitions.Add(grdLogButtons);            
            logLayout.HorizontalAlignment = HorizontalAlignment.Stretch;
            logLayout.VerticalAlignment = VerticalAlignment.Stretch;

            TextBlock spacer1 = new TextBlock();
            spacer1.Height = 10;
            
            TextBlock spacer2 = new TextBlock();
            spacer2.Height = 10;

            TextBlock spacer3 = new TextBlock();
            spacer3.Width = 10;

            TextBlock spacer4 = new TextBlock();
            spacer4.Width = 10;

            StackPanel ticketButtons = new StackPanel();
            ticketButtons.Orientation = Orientation.Horizontal;
            ticketButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
            ticketButtons.VerticalAlignment = VerticalAlignment.Stretch;            

            StackPanel ticketContent = new StackPanel();
            ticketContent.Orientation = Orientation.Vertical;
            ticketContent.HorizontalAlignment = HorizontalAlignment.Stretch;
            ticketContent.VerticalAlignment = VerticalAlignment.Stretch;

            StackPanel logButtons = new StackPanel();
            logButtons.Orientation = Orientation.Horizontal;
            logButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
            logButtons.VerticalAlignment = VerticalAlignment.Stretch;

            TextBox ticketSubject = new TextBox();
            ticketSubject.Name = "ticketSubject" + dynamicIndexPosition;
            ticketSubject.VerticalAlignment = VerticalAlignment.Stretch;
            ticketSubject.HorizontalAlignment = HorizontalAlignment.Stretch;
            ticketSubject.Text = CurrentData.callLogData[listPosition].Item4 + " | Ticket Number: " + CurrentData.callLogData[listPosition].Item1;

            TextBox callLogText = new TextBox();
            callLogText.Width = 1000;
            callLogText.Name = "callLogText" + dynamicIndexPosition;
            callLogText.TextWrapping = TextWrapping.Wrap;
            callLogText.Text = "Call Log Date: " + CurrentData.callLogData[listPosition].Item7 + " | Created By: " + CurrentData.callLogData[listPosition].Item6 + "\n \n" + CurrentData.callLogData[listPosition].Item5;
            callLogText.VerticalAlignment = VerticalAlignment.Stretch;

            Button prevTicket = new Button();
            prevTicket.Content = " Prev Ticket ";
            prevTicket.HorizontalAlignment = HorizontalAlignment.Left;
            prevTicket.Name = "prevTicket" +dynamicIndexPosition;
            

            Button nextTicket = new Button();
            nextTicket.Content = " Next Ticket ";
            nextTicket.HorizontalAlignment = HorizontalAlignment.Right;
            nextTicket.Name = "nextTicket" + dynamicIndexPosition;
            

            Button prevLog = new Button();
            prevLog.Content = " Prev Log ";
            prevLog.HorizontalAlignment = HorizontalAlignment.Left;
            prevLog.Name = "prevLog" +dynamicIndexPosition;
            

            Button nextLog = new Button();
            nextLog.Content = " Next Log ";
            nextLog.HorizontalAlignment = HorizontalAlignment.Right;
            nextLog.Name = "nextLog" +dynamicIndexPosition;
            

            ScrollViewer scrollCallLogContent = new ScrollViewer();
            scrollCallLogContent.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollCallLogContent.Height = 300;
            scrollCallLogContent.HorizontalAlignment = HorizontalAlignment.Stretch;
            scrollCallLogContent.Content = callLogText;

            ticketButtons.Children.Add(prevTicket);
            ticketButtons.Children.Add(spacer3);
            ticketButtons.Children.Add(nextTicket);

            ticketContent.Children.Add(ticketSubject);
            ticketContent.Children.Add(scrollCallLogContent);

            logButtons.Children.Add(prevLog);
            logButtons.Children.Add(spacer4);
            logButtons.Children.Add(nextLog);

            

            Grid.SetRow(ticketButtons, 0);
            Grid.SetRow(spacer1, 1);
            Grid.SetRow(ticketContent, 2);
            Grid.SetRow(spacer2, 3);
            Grid.SetRow(logButtons, 4);

            logLayout.Children.Add(ticketButtons);
            logLayout.Children.Add(spacer1);
            logLayout.Children.Add(ticketContent);
            logLayout.Children.Add(spacer2);
            logLayout.Children.Add(logButtons);

            prevTicket.Click += (sender, EventArgs) => { _log_ticketClickEvent(sender, EventArgs, listPosition, prevTicket.Name, ticketSubject, callLogText, prevTicket, nextTicket, prevLog, nextLog); };
            nextTicket.Click += (sender, EventArgs) => { _log_ticketClickEvent(sender, EventArgs, listPosition, nextTicket.Name, ticketSubject, callLogText, prevTicket, nextTicket, prevLog, nextLog); };
            prevLog.Click += (sender, EventArgs) => { _log_ticketClickEvent(sender, EventArgs, listPosition, prevLog.Name, ticketSubject, callLogText, prevTicket, nextTicket, prevLog, nextLog); };
            nextLog.Click += (sender, EventArgs) => { _log_ticketClickEvent(sender, EventArgs, listPosition, nextLog.Name, ticketSubject, callLogText, prevTicket, nextTicket, prevLog, nextLog); };

            _setButtons(listPosition, nextTicket, prevTicket, nextLog, prevLog);

            //Console.WriteLine("list " + listPosition + " dynamicposition " + dynamicIndexPosition + " " + CurrentData.callLogData[listPosition].Item3 + " total count " + CurrentData.callLogData.Count);

            return logLayout;
        }


        private void _log_ticketClickEvent(object sender, RoutedEventArgs e, int listPosition, string buttonName, TextBox ticketSubject, TextBox callLogText, Button prevTicket, Button nextTicket, Button prevLog, Button nextLog)
        {
            _log_ticketClick(listPosition, buttonName, ticketSubject, callLogText, prevTicket, nextTicket, prevLog, nextLog);
            
        }

        private void _log_ticketClick(int positionNumber, string buttonName, TextBox ticketSubject, TextBox callLogText, Button prevTicket, Button nextTicket, Button prevLog, Button nextLog)
        {
            
            currentCategory = CurrentData.callLogData[positionNumber].Item3;
            Console.WriteLine("Current Pos: " + currentPosition);
            if (CurrentData.callLogData[currentPosition].Item3 !=currentCategory)
            {
                resetCurrentPosition = true;
            }
            if( resetCurrentPosition == true)
            {
                currentPosition = positionNumber;
                resetCurrentPosition = false;
            }
            if(resetCurrentPosition == false)
            {
                if(buttonName.Contains("Ticket"))
                {
                    _setTicketButtons(buttonName, nextTicket, prevTicket, nextLog, prevLog, ticketSubject, callLogText);
                }
                else if (buttonName.Contains("Log"))
                {
                    _setLogButtons(buttonName, nextLog, prevLog, callLogText);
                }  
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if(setStartDate == false || setEndDate == false)
            {
                MessageBox.Show("Either the start or end date is empty please select a date", "Date is empty");
            }
            else
            {
                currentPosition = 0;
                resetCurrentPosition = true;
                Results_Grid.Children.Clear();
                resultsBoxContent.Items.Clear();
                CurrentData.Login();
                CurrentData.PullCounts(StartDate.DisplayDate.Date, EndDate.DisplayDate.Date);
                CurrentData.PullLogs(StartDate.DisplayDate.Date, EndDate.DisplayDate.Date);
                AddResultsToGrid();
                CurrentData.Logoff();
            }
        }

        private void _setButtons(int position, Button nextTicket, Button prevTicket, Button nextLog, Button prevLog)
        {
            Console.WriteLine(CurrentData.callLogData.Count + " : " + position);
            if (position >= (CurrentData.callLogData.Count - 1))
            {
                Console.WriteLine(position + "END");
                nextTicket.IsEnabled = false;
                nextLog.IsEnabled = false;
            }
            if (position == 0)
            {
                Console.WriteLine(position + "END");
                prevTicket.IsEnabled = false;
                prevLog.IsEnabled = false;
            }
            else
            {
                try
                {
                    if (CurrentData.callLogData[position + 1].Item3 == CurrentData.callLogData[position].Item3)
                    {
                        nextTicket.IsEnabled = true;
                    }
                    else
                    {
                        nextTicket.IsEnabled = false;
                    }

                    if (CurrentData.callLogData[position - 1].Item3 == CurrentData.callLogData[position].Item3)
                    {
                        prevTicket.IsEnabled = true;
                    }
                    else
                    {
                        prevTicket.IsEnabled = false;
                    }

                    if (CurrentData.callLogData[position + 1].Item1 == CurrentData.callLogData[position].Item1)
                    {
                        nextLog.IsEnabled = true;
                    }
                    else
                    {
                        nextLog.IsEnabled = false;
                    }

                    if (CurrentData.callLogData[position - 1].Item1 == CurrentData.callLogData[position].Item1)
                    {
                        prevLog.IsEnabled = true;
                    }
                    else
                    {
                        prevLog.IsEnabled = false;
                    }
                }
                catch(Exception ex) { Trace.TraceError(ex.ToString()); }
            }
        }

        private void _setTicketButtons(string buttonName, Button nextTicket, Button prevTicket, Button nextLog, Button prevLog, TextBox ticketSubject, TextBox callLogText)
        {
            if (buttonName.Contains("next"))
            {
                try
                {
                    while (CurrentData.callLogData[currentPosition + 1].Item1 == CurrentData.callLogData[currentPosition].Item1)
                    {
                        currentPosition++;
                        _setButtons(currentPosition, nextTicket, prevTicket, nextLog, prevLog);
                    }
                    if (CurrentData.callLogData[currentPosition + 1].Item3 == CurrentData.callLogData[currentPosition].Item3)
                    {
                        currentPosition++;
                        while (CurrentData.callLogData[currentPosition + 1].Item1 == CurrentData.callLogData[currentPosition].Item1)
                        {
                            currentPosition++;
                            _setButtons(currentPosition, nextTicket, prevTicket, nextLog, prevLog);
                        }
                        ticketSubject.Text = CurrentData.callLogData[currentPosition].Item4 + " | Ticket Number: " + CurrentData.callLogData[currentPosition].Item1;
                        callLogText.Text = "Call Log Date: " + CurrentData.callLogData[currentPosition].Item7 + " | Created By: " + CurrentData.callLogData[currentPosition].Item6  + "\n \n" + CurrentData.callLogData[currentPosition].Item5;
                        _setButtons(currentPosition, nextTicket, prevTicket, nextLog, prevLog);

                    }
                    else
                    {
                        Console.WriteLine(buttonName + " pos = " + currentPosition);
                        nextTicket.IsEnabled = false;

                    }
                }
                catch(Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    Console.WriteLine(currentPosition);
                    ticketSubject.Text = CurrentData.callLogData[currentPosition].Item4 + " | Ticket Number: " + CurrentData.callLogData[currentPosition].Item1;
                    callLogText.Text = "Call Log Date: " + CurrentData.callLogData[currentPosition].Item7 + " | Created By: " + CurrentData.callLogData[currentPosition].Item6 + "\n \n" + CurrentData.callLogData[currentPosition].Item5;
                    nextTicket.IsEnabled = false;
                    nextLog.IsEnabled = false;
                }
            }
            else
            {
                try
                {
                    Console.WriteLine(buttonName);
                    while (CurrentData.callLogData[currentPosition - 1].Item1 == CurrentData.callLogData[currentPosition].Item1)
                    {
                        currentPosition--;
                        _setButtons(currentPosition, nextTicket, prevTicket, nextLog, prevLog);
                        Console.WriteLine("While Loop 1 " + currentPosition);

                    }
                    if (CurrentData.callLogData[currentPosition - 1].Item3 == CurrentData.callLogData[currentPosition].Item3)
                    {
                        currentPosition--;
                        Console.WriteLine("If statement before while loop 2  " + currentPosition);
                        while (CurrentData.callLogData[currentPosition + 1].Item1 == CurrentData.callLogData[currentPosition].Item1)
                        {
                            currentPosition++;
                            _setButtons(currentPosition, nextTicket, prevTicket, nextLog, prevLog);
                            Console.WriteLine("While Loop 2 " + currentPosition);
                        }
                        Console.WriteLine("Post While loop " + currentPosition);
                        Console.WriteLine(buttonName + " pos = " + currentPosition);
                        ticketSubject.Text = CurrentData.callLogData[currentPosition].Item4 + " | Ticket Number: " + CurrentData.callLogData[currentPosition].Item1;
                        callLogText.Text = "Call Log Date: " + CurrentData.callLogData[currentPosition].Item7 + " | Created By: " + CurrentData.callLogData[currentPosition].Item6 + "\n \n" + CurrentData.callLogData[currentPosition].Item5;
                        _setButtons(currentPosition, nextTicket, prevTicket, nextLog, prevLog);
                    }
                    else
                    {
                        Console.WriteLine("ELSE " + buttonName + " pos = " + currentPosition);
                        //buttonName.IsEnabled = false;
                        prevTicket.IsEnabled = false;
                    }
                    if (CurrentData.callLogData[currentPosition - 1].Item1 == CurrentData.callLogData[currentPosition].Item1)
                    {
                        int tempPosition = currentPosition;
                        while (CurrentData.callLogData[tempPosition - 1].Item1 == CurrentData.callLogData[tempPosition].Item1)
                        {
                            tempPosition--;
                            _setButtons(currentPosition, nextTicket, prevTicket, nextLog, prevLog);
                            Console.WriteLine("TempLoop 1 " + tempPosition);

                        }
                        Console.WriteLine("TempPosition 1 " + tempPosition);
                        if (CurrentData.callLogData[tempPosition - 1].Item3 != CurrentData.callLogData[tempPosition].Item3)
                        {
                            prevTicket.IsEnabled = false;
                        }
                    }
                    
                }
                catch(Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    Console.WriteLine(currentPosition);
                    ticketSubject.Text = CurrentData.callLogData[currentPosition].Item4 + " | Ticket Number: " + CurrentData.callLogData[currentPosition].Item1;
                    callLogText.Text = "Call Log Date: " + CurrentData.callLogData[currentPosition].Item7 + " | Created By: " + CurrentData.callLogData[currentPosition].Item6 + "\n \n" + CurrentData.callLogData[currentPosition].Item5;
                    prevTicket.IsEnabled = false;
                    prevLog.IsEnabled = false;
                }
            }
        }

        private void _setLogButtons(string buttonName, Button nextLog, Button prevLog, TextBox callLogText)
        {
            if (buttonName.Contains("next"))
            {
                try
                {
                    if (CurrentData.callLogData[currentPosition + 1].Item1 == CurrentData.callLogData[currentPosition].Item1)
                    {
                        currentPosition++;
                        callLogText.Text = "Call Log Date: " + CurrentData.callLogData[currentPosition].Item7 + " | Created By: " + CurrentData.callLogData[currentPosition].Item6 + "\n \n" + CurrentData.callLogData[currentPosition].Item5;
                    }
                    else
                    {
                        nextLog.IsEnabled = false;
                    }
                    if (CurrentData.callLogData[currentPosition + 1].Item1 != CurrentData.callLogData[currentPosition].Item1)
                    {
                        nextLog.IsEnabled = false;
                    }
                    if (CurrentData.callLogData[currentPosition - 1].Item1 == CurrentData.callLogData[currentPosition].Item1)
                    {
                        prevLog.IsEnabled = true ;
                    }
                }
                catch(Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    Console.WriteLine(currentPosition);
                    callLogText.Text = CurrentData.callLogData[currentPosition].Item5;
                    nextLog.IsEnabled = false;
                }
            }
            else
            {
                try
                {
                    if (CurrentData.callLogData[currentPosition - 1].Item1 == CurrentData.callLogData[currentPosition].Item1)
                    {
                        currentPosition--;
                        callLogText.Text = "Call Log Date: " + CurrentData.callLogData[currentPosition].Item7 + " | Created By: " + CurrentData.callLogData[currentPosition].Item6 + "\n \n" + CurrentData.callLogData[currentPosition].Item5;
                    }
                    else
                    {
                        prevLog.IsEnabled = false;
                    }
                    if (CurrentData.callLogData[currentPosition - 1].Item1 != CurrentData.callLogData[currentPosition].Item1)
                    {
                        prevLog.IsEnabled = false;
                    }
                    if (CurrentData.callLogData[currentPosition + 1].Item1 == CurrentData.callLogData[currentPosition].Item1)
                    {
                        nextLog.IsEnabled = true;
                    }
                }
                catch(Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    Console.WriteLine(currentPosition);
                    callLogText.Text = CurrentData.callLogData[currentPosition].Item5;
                    prevLog.IsEnabled = false;
                }
            }
        }
    }

  
}
