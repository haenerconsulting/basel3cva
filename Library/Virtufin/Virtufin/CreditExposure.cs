using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;

namespace Virtufin
{
    [Guid("5C55DA1B-7AE3-4AD4-96F0-F21373FA127B")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class CreditExposure
    {
        private static void ErrorMessage(String msg)
        {
            MessageBox.Show(msg, "Error");
        }
        private static T[,] RangeToTypedArray<T>(Excel.Range range)
        {
            var v = range.Value2 as object[,];
            var err = false;
            if(v==null)
            {
                return new T[0,0];
            }
            var result = new T[v.GetLength(0),v.GetLength(1)];
            for(var i=0;i<v.GetLength(0);i++)
            {
                for(var j=0;j<v.GetLength(1);j++)
                {
                    try
                    {
                        result[i, j] = (T) v[i+1, j+1];
                    }
                    catch (Exception)
                    {
                        
                        ErrorMessage(String.Format("Can't cast to a {0} array: {1} is not a {2}",typeof(T).Name,v[i+1,j+1],typeof(T).Name));
                        err = true;
                        break;
                    }
                }
                if (err) break;
            }
            return result;
        }
        public double[,] MatrixTranspose(Excel.Range matrix)
        {

            var v = RangeToTypedArray<double>(matrix);
            var m = new DenseMatrix(v);
            try
            {
                return m.Transpose().ToArray();
            }
            catch (Exception e)
            {

                ErrorMessage(e.ToString());
                return null;
            }
        }
        public double[,] Multiply(Excel.Range matrix1,Excel.Range matrix2)
        {

            var v1 = RangeToTypedArray<double>(matrix1);
            var m1 = new DenseMatrix(v1);
            var v2 = RangeToTypedArray<double>(matrix2);
            var m2 = new DenseMatrix(v2);
            try
            {
                return m1.Multiply(m2).ToArray();
            }
            catch (Exception e)
            {

                ErrorMessage(e.ToString());
                return null;
            }
        }
        public double[,] DiagonalizedMatrix(Excel.Range matrix)
        {

            var v = RangeToTypedArray<double>(matrix);
            var m = new DenseMatrix(v);
            try
            {
                var evd = m.Evd();
                if(!evd.IsSymmetric)
                {
                    throw new Exception("Matrix is not symmetric!");
                }
                var d = evd.EigenValues().Select(a => a.Real).ToArray();
                return new DiagonalMatrix(d.Length,d.Length,d).ToArray();
            }
            catch (Exception e)
            {

                ErrorMessage(e.ToString());
                return null;
            }
        }
        public double[,] EigenVectors(Excel.Range matrix)
        {

            var v = RangeToTypedArray<double>(matrix);
            var m = new DenseMatrix(v);
            try
            {
                var evd = m.Evd();
                if (!evd.IsSymmetric)
                {
                    throw new Exception("Matrix is not symmetric!");
                }
                return evd.EigenVectors().ToArray();
            }
            catch (Exception e)
            {

                ErrorMessage(e.ToString());
                return null;
            }
        }
        public double[,] Cholesky(Excel.Range matrix)
        {

            var v = RangeToTypedArray<double>(matrix);
            var m = new DenseMatrix(v);
            try
            {
                return new DenseCholesky(m).Factor.ToArray();
            }
            catch (Exception e)
            {
                
                ErrorMessage(e.ToString());
                return null;
            }
        }
        /**
         * See http://blogs.msdn.com/b/eric_carter/archive/2004/12/01/273127.aspx
         * 
         */

        public double AddNumbers(double n1, [Optional] object n2, [Optional] object n3)
        {
            double result = 0;
            result += Convert.ToDouble(n1);

            if (!(n2 is System.Reflection.Missing))
            {
                var r2 = n2 as Excel.Range;
                double d2 = Convert.ToDouble(r2.Value2);
                result += d2;
            }

            if (!(n3 is System.Reflection.Missing))
            {
                var r3 = n3 as Excel.Range;
                double d3 = Convert.ToDouble(r3.Value2);
                result += d3;
            }

            return result;
        }
        ///////////////////////////////////////////////////
        [ComRegisterFunctionAttribute]
        public static void RegisterFunction(Type type)
        {

            Registry.ClassesRoot.CreateSubKey(
              GetSubKeyName(type, "Programmable"));
            var key = Registry.ClassesRoot.OpenSubKey(
              GetSubKeyName(type, "InprocServer32"), true);
            key.SetValue("",
              System.Environment.SystemDirectory + @"\mscoree.dll",
              RegistryValueKind.String);
        }
        [ComUnregisterFunctionAttribute]
        public static void UnregisterFunction(Type type)
        {

            Registry.ClassesRoot.DeleteSubKey(
              GetSubKeyName(type, "Programmable"), false);
        }
        private static string GetSubKeyName(Type type,
          string subKeyName)
        {
            var s =
              new System.Text.StringBuilder();
            s.Append(@"CLSID\{");
            s.Append(type.GUID.ToString().ToUpper());
            s.Append(@"}\");
            s.Append(subKeyName);
            return s.ToString();
        }
    }
}
