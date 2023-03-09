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
using static System.Math;
using System.Data;

namespace Lab1Algorythm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Bisection();
            Steffensen(-4);
            Steffensen(-2);
            SimpleIteration(-3.5, -4);
            Lobachevskiy();
        }
        public void Bisection()
        {
            DataTable dT = new DataTable("Проміжні обчислення");
            dT.Columns.Add("Проміжні обчислення");
            double a = 1;
            double b = 0;
            double x=(a+b)/2;
            double middle=0;
            double func1, func2, func3;
            double k = Log(Abs(a - b) * 10000000, 2);
            for (int i = 0; i < Ceiling(k); i++)
            {
                if (i == 0)
                    middle = 0;
                else
                    middle = (a + b) / 2;
                x = middle;
                func1 = Function(middle);
                func2 = Function(a);
                func3 = Function(b);
                if (func1 < 0)
                {
                    if (func2 < 0 && func3 > 0)
                        a = middle;
                    if(func2>0 && func3<0)
                        b = middle;
                }
                else if (func1 == 0)
                {
                    x = middle;
                    return;
                }
                else
                {
                    if (func2 < 0 && func3 > 0)
                        b = middle;
                    if (func2 > 0 && func3 < 0)
                        a = middle;
                }
                dT.Rows.Add("x" + i + " = " + x);
            }
            result.Content ="x = " + x;
            task1.ItemsSource = dT.DefaultView;
        }
        public double Function(double x)
        {
            double function = x*x* Cos(x) + Log(Pow(E, x), 2) + PI - 9 * PI *x*x*x;
            return function;
        }
        public double Function1(double x)
        {
            double function = Pow(E, Cosh(x)) +Pow(x, 5) + Pow(x, 15)*Sin(x)- 13;
            return function;
        }
        public double Function1Derivative(double x)
        {
            return  Pow(E, Cosh(x))*Sinh(x)+ 5 * Pow(x, 4) + 15 * Pow(x, 14) * Sin(x) + Pow(x, 15) * Cos(x);
        }
        public double Function2(double x)
        {
            return Pow(E, Cosh(x))*x + Pow(x, 16) * Sin(x) + Pow(x, 6)-12*x;
        }
        public double Function2Derivative(double x)
        {
            return x*(Pow(E, Cosh(x)) * Sinh(x) + 16 * Pow(x, 15) * Sin(x) + Pow(x, 16) * Cos(x) + Pow(E, Cosh(x)) + 6*Pow(x, 5) -12);
        }
        DataTable dT1 = new DataTable();
        int counter = 0;
        public void Steffensen(double a)
        {
            if (counter==0)
                dT1.Columns.Add("Перший корінь");
            else
                dT1.Columns.Add("Другий корінь");
            int row = Newton(a).Item2;
            double xk= Newton(a).Item1, xk1=0;
            int column;
            if (counter == 0)
                column = 0;
            else
                column = 1;
            double b=10, c, epsilon=Pow(10,-6);
            int i = 0;
            while(Abs(b)>epsilon)
            {
                b = Function1(xk);
                c = Function1(xk + b);
                xk1 = xk - (b*b) / (c - b);
                xk = xk1;
                dT1.Rows.Add();
                dT1.Rows[row][column]="x" + i + " = " + xk1;
                row++;
                i++;
            }
            if(temp1.Content.ToString()=="Label")
                temp1.Content = "x1 = "+xk1;
            else
                temp3.Content = "x2 = "+xk1;
            counter++;
            task2.ItemsSource = dT1.DefaultView;
        }
        public (double, int) Newton(double a)
        {
            double xk1 = 0;
            double epsilon = Pow(10, -1);
            double x0 = a;
            double xk = x0;
            int i = 0;
            int column;
            if (counter == 0)
                column = 0;
            else
                column = 1;
            while(Abs(Function1(xk1)) > epsilon)
            {
                xk1 = xk - Function1(xk) / Function1Derivative(x0);
                x0 = xk;
                xk = xk1;
                dT1.Rows.Add();
                dT1.Rows[i][column] = "x" + i + " = " + xk1;
                i++;
            }
            return (xk1, i);
        }
        public void SimpleIteration(double a, double b)
        {
            double root = 0;
            double alpha = Math.Min(Function2Derivative(a), Function2Derivative(b));
            double gamma = Math.Max(Function2Derivative(a), Function2Derivative(b));
            double lambda = 2 / (gamma + alpha);
            double q = Math.Abs((gamma - alpha) / (gamma + alpha));
            double prew = a;
            double temp = b;
            double epsilon = Pow(10, -8);
            DataTable dT2 = new DataTable();
            dT2.Columns.Add("Корені рівняння");
            int i = 0;
            while (Abs(temp - prew) > (1 - q) / q * epsilon)
            {
                prew = temp;
                temp = prew - lambda * Function2(prew);
                root = temp;
                if (root == 0)
                {
                    task3x.Content = "немає коренів";
                    break;
                }
                i++;
                dT2.Rows.Add("x["+i+"] = "+root);
            }
            if(task3x.Content.ToString()=="Label")
                task3x.Content = "x = " + root;
            task3.ItemsSource = dT2.DefaultView;
        }
        public void Lobachevskiy()
        {
            DataTable dT3 = new DataTable();
            dT3.Columns.Add("Корені рівняння");
            double epsilon = Pow(10,-3), s = 0;
            double [] array = { 17, 268, 472, -837, -744, 414, 124, -34 };
            double [] a = {17, 268, 472, -837, -744, 414, 124,-34};
            double [] b = new double [a.Length];
            double [] borders = new double [a.Length-1];
            double max1=0, max2=0;
            int beta = 1;
            for (int i = a.Length - 2; i >= 0; i--)
            {
                borders[i] = Pow(Abs(a[a.Length - 1 - i]) / 17.0, 1 / Convert.ToDouble(beta));
                beta++;
            }
            for (int i = a.Length - 2; i >=0 ; i--)
            {
                if(max1<borders[i])
                    max1=borders[i];
                if(max2<borders[i] && max1!= borders[i])
                    max2=borders[i];
            }
            minvalue.Content = "Xmin = " + (-1)*(max1+max2);
            maxvalue.Content = "Xmax = " + (max1 + max2);
            int n = 7;
            double [] c = new double[a.Length];
            double k = 0, p = 0;
            int index = 0;
            do
            {
                k = 0;
                for (int i = 0; i <= n; i++)
                {
                    s = 0;
                    for(int j=1; j<=n-i; j++)
                    {
                        if (i - j >= 0 && i + j < a.Length)
                            s = s + Pow(-1, j) * a[i - j] * a[i + j];
                        else
                            break;
                    }
                    b[i] = a[i] * a[i] + 2 * s;
                }
                for (int j = 1; j < n; j++)
                {
                    k = k + Pow(1 - b[j] / (a[j] * a[j]), 2);
                }
                p = Sqrt(k);
                for (int i = 0; i <= n; i++)
                {
                    a[i] = b[i];
                }
                index++;
            } while (p > epsilon);
            int m=Convert.ToInt32(Pow(2, index));
            double x;
            double [] X=new double[b.Length-1];
            for(int i=1; i <= n; i++)
            {
                x = b[i-1] / b[i];
                X[i-1] = (-1) * Pow(Abs(x), 1 / Convert.ToDouble(m));
                dT3.Rows.Add("x[" + i + "] = " + X[i-1]);
            }
            task4.ItemsSource = dT3.DefaultView;
        }
    }
}
