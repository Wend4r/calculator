using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphics
{
	public partial class Calculator: Form
	{
		public Calculator()
		{
			InitializeComponent();
		}

		private void Graphics_Load(object sender, EventArgs e)
		{

			void Join(Button ButtonRef)
			{
				ButtonRef.Click += (a, b) =>
				{
					TextExample.Text += ButtonRef.Text;
				};
			};

			ButtonPlusMinus.Click += (a, b) =>
			{
				if(TextExample.Text.Length != 0)
				{
					Match hMatch = new Regex(@"\-\((.*?)\)").Match(TextExample.Text);
					TextExample.Text = hMatch.Length > 1 ? hMatch.Groups[1].Value : ("-(" + TextExample.Text + ')');
				}
			};

			ButtonClear.Click += (a, b) =>
			{
				TextExample.Text = "";
			};

			ButtonBackspace.Click += (a, b) =>
			{
				int iLen = TextExample.Text.Length;

				if(iLen != 0)
				{
					TextExample.Text = TextExample.Text.Remove(iLen - 1);
				}
			};

			Join(Button1);
			Join(Button2);
			Join(Button3);
			Join(Button4);
			Join(Button5);
			Join(Button6);
			Join(Button7);
			Join(Button8);
			Join(Button9);

			Join(Button0);
			Join(ButtonComma);
			Join(ButtonPlus);

			Join(ButtonDivide);

			ButtonMultiply.Click += (a, b) =>
			{
				TextExample.Text += '*';
			};

			ButtonMinus.Click += (a, b) =>
			{
				TextExample.Text += '-';
			};

			ButtonResult.Click += (a, b) =>
			{
				if(TextExample.Text != "")
				{
					TextExample.Text = Calculat(TextExample.Text);
				}
			};

			Join(ButtonBracket1);
			Join(ButtonBracket2);
			Join(ButtonPrecent);
			Join(ButtonDegrees);

			ButtonSqrt.Click += (a, b) =>
			{
				TextExample.Text += TextExample.Text.Length > 0 ? "s" : "2s";
			};

			TextExample.KeyDown += (a, Key) =>
			{
				if(Key.KeyCode == Keys.Enter && TextExample.Text != "")
				{
					TextExample.Text = Calculat(TextExample.Text);
					Key.Handled = true;
					Key.SuppressKeyPress = true;
				}
			};

			TextExample.TextChanged += (a, b) =>
			{
				//if(!Regex.IsMatch(TextExample.Text, @"([0-9])"))
				//{
				//	TextExample.Text = TextExample.Text.Remove(TextExample.Text.Length - 1);
				//}
			};
		}

		static string Calculat(string strExample)
		{
			Match hMatch = new Regex(@"\(((?>[^\(\)]+|\((?<c>)|\)(?<-c>))*(?(c)(?!)))").Match(strExample);

			for(int ji = 0, iCountPattern = hMatch.Length; ji != iCountPattern;)
			{
				string strBuf = hMatch.Groups[++ji].Value;
				strExample = strExample.Replace('(' + strBuf + ')', Calculat(strBuf));
			}

			char[]   sOperator = {'%', '^', '*', '/' , 's', '-', '+'};

			int      iLen = strExample.Length,
					 iOpers = sOperator.Length+1;

			byte     iCount = 1,
					 iOperMin = (byte)iOpers;

			bool[,]  bIsOperator = new bool[iOpers, 256/iOpers];
			

			for(int j = 0; j != iLen; j++)
			{
				char sSymbol = strExample[j];

				if(!((41 < sSymbol || sSymbol == ' ' || sSymbol == '%') && (sSymbol < 58 || sSymbol == 's' || sSymbol == '^'))) // Под рерулярку не закинул, потому что ъыъ
				{
					return "Ошибка";
				}

				for(byte j2 = 0; j2 != iOpers-1; j2++)
				{
					if(sSymbol == sOperator[j2])
					{
						if(j2 < iOperMin)
						{
							iOperMin = j2;
						}
						bIsOperator[j2, iCount++] = true;
						break;
					}
				}
			}

			string[] strNumbers = strExample.Replace('.', ',').Split(sOperator);

			double[] iValue = new double[strNumbers.Length];

			int      i = 0;

			foreach(string strNumber in strNumbers)
			{
				iValue[i++] = strNumber != "" ? Convert.ToDouble(strNumber) : 0;
			}
			
			for(i = iOperMin; i != iOpers; i++)
			{
				for(int i2 = 1; i2 != iCount; i2++)
				{
					if(bIsOperator[i, i2])
					{
						switch(i)
						{
							case 0:     // '%'
							{
								iValue[i2 - 1] /= 100;
								break;
							}
							case 1:     // '^'
							{
								iValue[i2 - 1] = Math.Pow(iValue[i2 - 1], iValue[i2]);
								break;
							}
							case 2:     // '*'
							{
								iValue[i2 - 1] *= iValue[i2];
								break;
							}
							case 3:     // '/'
							{
								iValue[i2 - 1] /= iValue[i2];
								break;
							}
							case 4:     // 's'
							{
								iValue[i2 - 1] = Math.Pow(iValue[i2], 1 / iValue[i2 - 1]);
								break;
							}
							case 5:     // '-'
							{
								iValue[i2 - 1] -= iValue[i2];
								break;
							}
							case 6:     // '+'
							{
								iValue[i2 - 1] += iValue[i2];
								break;
							}
						}
						iValue[i2] = iValue[i2 - 1];
					}
				}
			}

			return iValue[iCount - (iCount > 1 ? 2 : 1)].ToString();
		}

		private void ButtonPlusMinus_Click(object sender, EventArgs e)
		{

		}
	}
}