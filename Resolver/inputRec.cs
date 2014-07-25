using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resolver
{
    public class inputRec
    {

        int getLastBracket(string i, int maxindex) //ottiene l'indice della parentesi più interna
        {
            int index=1; //si può avre la parentesi con indice 1 solo se c'è un segno davanti (improbabile)
            for (int k = 0; k < i.Length; k++)
            {
                if (i[k] == '(')
                {
                    index = k;
                }
            }
            return index;
        }
        string basicExpression(string i) //espressioni veloci senza parentesi con i 4 operatori fondamentali------------------------------Attenzione! solo numeri interi
        {
            string resultstr=null;
            if(i.Contains('*'))
            {
                while(true)
                {
                    int j, r, u;
                    string x = "", y = "";
                    //find the length of both the numbers that surround the operator
                    int length1 = 0, length2 = 0, firstnumberstart, secondnumberend;
                    j = i.IndexOf('*') - 1;
                    firstnumberstart = j;
                    while ((j >= 0) && (isNumber(i[j]))) { length1++; j--; } //number on the left
                    j = i.IndexOf('*') + 1;
                    while ((j < i.Length) && (isNumber(i[j]))) { length2++; j++; } //numero on the right
                    secondnumberend = j - 1;
                    //merge the digits to get the numbers
                    j = i.IndexOf('*') - length1;
                    for (int temp = j; temp < (length1 + j); temp++)
                    {
                        x += i[temp];
                    }
                    j = i.IndexOf('*') + 1;
                    for (int temp = j; temp < (length2 + j); temp++)
                    {
                        y += i[temp];
                    }
                    r = Convert.ToInt32(x) * Convert.ToInt32(y);
                    //replace the result in the string
                    char[] ic = new char[i.Length];
                    char[] result = new char[i.Length];
                    ic = i.ToCharArray();
                    for (u = 0; u < r.ToString().Length; u++)
                    {
                        ic[firstnumberstart + u] = r.ToString().ToCharArray()[u];
                    }
                    //remove the rest of the expression
                    int ind = 0;
                    for (int temp = 0; temp < i.Length; temp++ )
                    {
                        if((temp > u) && (temp < secondnumberend))
                        {
                            temp++;
                        }
                        else
                        {
                            result[ind] = ic[temp];
                            ind++;
                        }
                    }
                    resultstr = "";
                    for (int temp = 0; temp < result.Length; temp++)
                    {
                        resultstr += result[temp];
                    }
                    //se ci sono altre moltiplicazioni continua a ciclare altrimenti break;
                    if (Contains(i, '*', j))
                        j = i.IndexOf('*', j + 1);
                    else
                        break;
                }
            }
            return resultstr;
        }
        bool isNumber(char c)
        {
            if ((c >= '0') && (c <= '9'))
                return true;
            else
                return false;
        }
        bool Contains(string i, char c, int startIndex = 0)
        {
            for(int j = startIndex; j < i.Length; j++)
            {
                if (i[j] == c)
                    return true;
            }
            return false;
        }

        public string getValue(string i)
        {
            //testcode
            return basicExpression(i);
        }
    }
}