using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int[] array;
        private int i = 0;
        private int j;
        private int key;
        private bool haveKey;
        

        public Form1()
        {
            InitializeComponent();
        }

        protected void DrawArrow( Graphics graphics, Rectangle rectangle, int fromCol, int toCol, int fromRow, int toRow, bool arrowStart)
        {
            float width = rectangle.Width;
            float height = rectangle.Height;

            Pen pen = new Pen(Color.DarkRed, 7);
           

            if (arrowStart)
            {
                pen.StartCap = LineCap.ArrowAnchor;
            } 
            else
            {
                pen.EndCap = LineCap.ArrowAnchor;
            }

            graphics.DrawLine(
                pen, 
                width / 30 + (width / 15) * fromCol + fromCol * 10 ,
                height / 2 + ((width / 8) * fromRow) / 6,
                width / 30 + (width / 15) * toCol + toCol * 10,
                height / 2 + ((width / 8) *  toRow) / 6
                );
        }


        protected void DrawArrayBoxes(int[] arr, Graphics graphics, Rectangle clientRectangle, int cell, int line, int[] blueRange, int[] pinkRange)
        {
            float width = clientRectangle.Width;
            float height = clientRectangle.Height;
            

            for (int index = 0; index < arr.Length; ++index)
            {
                RectangleF rectangleF = new RectangleF(
                    width / 30 + (width / 15) * (index + cell) + 6 * (index + cell),
                    height / 4 + 30 * line,
                    width / 15,
                    height / 7);

                graphics.FillRectangle(Brushes.White, rectangleF);

                if (blueRange.Contains(index))
                {
                    graphics.FillRectangle(Brushes.LightBlue, rectangleF);
                }


                if (pinkRange.Contains(index))
                {
                    graphics.FillRectangle(Brushes.Pink, rectangleF);
                }


                graphics.DrawString(
                            arr[index].ToString(),
                            new Font("Time New Rome", 16),
                            Brushes.Black,
                            new RectangleF( 
                                width / 30 + width / 15 * (index + cell) + 6 * (index + cell),
                                height / 4 + 30 * line + 25,
                                width / 15,
                                height / 7 + 25),
                StringFormat.GenericDefault);


                graphics.DrawRectangle(
                           new Pen(Brushes.Black),
                           rectangleF.X,
                           rectangleF.Y,
                           rectangleF.Width,
                           rectangleF.Height);
            }
        }


        private void buttonNext_Click(object sender, EventArgs e)
        {
           
            if (array == null)
            {
                i = 0;
                j = 0;
                haveKey = false;
                array = textBoxNums.Text
                    .Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s))
                    .ToArray();

                textBoxNums.Visible = false;
            }

            bool upArrow = false;
            bool downArrow = false;

            Invalidate();
            Application.DoEvents();
            Graphics graphics = CreateGraphics();

            if (i < array.Length)
            {
             
                if (j >= 0 && haveKey && array[j] >= key)
                {
                    array[j + 1] = array[j];
                    DrawArrayBoxes(array, graphics, ClientRectangle, 0, 3, Enumerable.Range(0, i + 1).ToArray(), new int[1] { j + 1 });
                    DrawArrow( graphics, ClientRectangle, j, j + 1, 0, 0, false);

                    j--;
                }
                else if (haveKey)
                {
                    array[j + 1] = key;
                    DrawArrayBoxes(array, graphics, ClientRectangle, 0, 3, Enumerable.Range(0, i + 1).ToArray(), new int[1] { j + 1 });

                    haveKey = false;
                    downArrow = true;
                }
                else
                {
                    i++;
                    DrawArrayBoxes(array, graphics, ClientRectangle, 0, 3, Enumerable.Range(0, i + 1).ToArray(), new int[0]);

                    if (i == array.Length)
                    {
                        return;
                    }

                    key = array[i];
                    haveKey = true;
                    upArrow = true;
                    j = i - 1;
                }


                DrawArrayBoxes(new int[1] { key }, graphics, ClientRectangle, j + 1, 1, new int[0], upArrow ? new int[1] : new int[0]);

                if (!upArrow && !downArrow)
                {
                    return;
                }

                DrawArrow( graphics, ClientRectangle, j + 1, j + 1, upArrow ? 0 : 1, downArrow ? 0 : 1, true);


            }
            else
            {
                array = null;
                textBoxNums.Visible = true;
            }

        }
    }
}
