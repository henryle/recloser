using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Infragistics.Win;

namespace RecloserAcq
{
    public class AlertCtrl : Control
    {
        
        private int _counter;

        private bool _alert;
        public bool Alert
        {
            get { return _alert; }
            set
            {
                _alert = value;
                Invalidate();
                //_timer.Enabled = _alert;
                //if (!_alert)                
                //{
                //    _counter = 0;
                //    Invalidate();
                //}
            }
        }

        public AlertCtrl():base()
        {
            this.Size = new Size(10, 10);

            //_timer = new Timer();
            //_timer.Interval = 1000;
            //_timer.Tick += new EventHandler(OnTimer);
        }

        void OnTimer(object sender, EventArgs e)
        {   
            _counter++;
            if (_counter > 99) _counter = 0;            

            Invalidate();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Disposing) return;

            var g = e.Graphics;
            g.Clear(Color.White);

            var rw = this.Size.Width;
            if (rw > this.Size.Height)
                rw = this.Size.Height;
            
            var x = (this.Width - rw)/2; 
            if(x < 0) x = 0;

            var y = (this.Height -rw)/2;
            if(y < 0) y = 0;

            var rect = new Rectangle(x, y, rw, rw);
            rect.Inflate(-1, -1);

            if (_alert)
                g.FillEllipse(Brushes.Red, rect);
            else
                g.FillEllipse(Brushes.White, rect); 

            //if (_counter % 2 == 1)
            //    g.FillEllipse(Brushes.Red, rect);
            //else
            //    g.FillEllipse(Brushes.White, rect);            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing )
            {
                
            }

            base.Dispose(disposing);
        }
    }
}
