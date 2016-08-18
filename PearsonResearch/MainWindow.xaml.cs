using PearsonResearch.ViewModel;
using System;
using System.Collections.Generic;
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

namespace PearsonResearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;


            //test what we have

            //binding doesn't work ? it works :P
            //todo : validate against spaces in parameter name
            ViewModel.AllParametersList.Add(new DataManagment.Parameter() { Name = "y" });
            ViewModel.AllParametersList.Add(new DataManagment.Parameter() { Name = "x" });
            ViewModel.LeftExpression = "[y]";
            ViewModel.RightExpression = "[x]";
            

        }


        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty , value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel" , typeof(MainWindowViewModel) , typeof(MainWindow) , new PropertyMetadata(default(MainWindowViewModel)));

        private void UpdateExpressionsButton_Click(object sender , RoutedEventArgs e)
        {
            foreach (var item in ViewModel.DataEnteries)
            {
                item.Left_Parameter.RaisePropertyChanged(nameof(item.Left_Parameter.MathematicalRepresentation));
                item.Left_Parameter.RaisePropertyChanged(nameof(item.Left_Parameter.Value));

                item.Right_Parameter.RaisePropertyChanged(nameof(item.Right_Parameter.MathematicalRepresentation));
                item.Right_Parameter.RaisePropertyChanged(nameof(item.Right_Parameter.Value));
            }
        }

        /*
         Pearson's formula : 
         c = data count

         R =
         (c * sum[x * y]) - (sum[x]*sum[y])
         /
         sqrt(c*sum[ x^2 ] - sum[x] ^ 2) * sqrt(c * sum[ y^2 ] - sum[y] ^ 2 )

        x and y can be custom variables
        Symbol : ∝

        what will we need :

        custom variables :
        ==================
        an object that has (variable name, variable desc) > PParameter that contains Parameter
        the operation done between variables - done
        ==================

        actual handling
        ================
        user will input his custom variables
        e.g. 
        row 1 in Data entries
        Parameter 1 , Prameter 2, Parameter 3 
        Value : 5   , Value : 1564, Value 0

        row 2 in Data entries
        Parameter 1 , Prameter 2, Parameter 3 
        Value : 424   , Value : 11, Value 54

        row 3 in Data entries
        Parameter 1 , Prameter 2, Parameter 3 
        Value : 7   , Value : 1, Value 78
        =================
         Program will do that          :

        get a value indicating the x&y value for each Data entry
        sum : 
        x

        y

        x * y

        x ^ 2
        y ^ 2

        so each data entry must have a property that contains x & y values
        =================
        we will also nead a data entry list
        ===========================

        Left = A * Right + B
        x = right , y = left

        A = 
        c * sum[left * right] - sum[left] * sum[right]
        /
        c * sum[Right ^ 2] - sum[Right] ^ 2

        B = 
        sum[left] - A * sum[right]
        / 
        c



                  */
    }
}
