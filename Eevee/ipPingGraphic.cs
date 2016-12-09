using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eevee
{
    class ipPingGraphic 
    {
        ccIPAddress Source;
        ccIPAddress Destination;
        float windowWidth;
        float windowHeight;
        double val = 1.0;
        public double prog = 0.0;
        double dist = 0.0;

        public ipPingGraphic next;

        public ipPingGraphic(ccIPAddress s, ccIPAddress d,  Point size)
        {
            Source = s;
            Destination = d;
            windowWidth = size.X;
            windowHeight = size.Y;

            dist = Math.Sqrt( Math.Pow((s.getLat() - d.getLat()), 2) + Math.Pow((s.getLon() - d.getLon()), 2));
            Console.WriteLine(dist);
        }

        public void start(Graphics g)
        {
            
            val = (dist * prog);
            double lat = -((1- val)*Source.getLat() +val* Destination.getLat() ) * (Convert.ToDouble(windowHeight) / 180);
            double lon = ((1-val)*Source.getLon() +val* Destination.getLon())  * (Convert.ToDouble(windowWidth) / 360);
           // double lat2 = -latOut * (Convert.ToDouble(panel1.Height) / 180);
          //  double lon2 = longOut * (Convert.ToDouble(panel1.Width) / 360);
            g.FillEllipse(Brushes.MediumOrchid, (float)((lon + ((Convert.ToDouble(windowWidth) / 360) * 180))), (float)((lat + (90 * (Convert.ToDouble(windowHeight) / 180)))), 20, 20);
            // grap.FillEllipse(Brushes.OrangeRed, (float)((lon2 + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(panel1.Height) / 180)))), 5, 5);
            //  grap.DrawLine(Pens.Aqua, (float)((lon + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat + (90 * (Convert.ToDouble(panel1.Height) / 180)))), (float)((lon2 + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(panel1.Height) / 180)))));
            Console.WriteLine((Source.getLat() + Destination.getLat()) / val+"}"+lat + " : " + lon + "> " + Source.getLat() + ":"+ Source.getLon());
            prog += .01;
            if(prog >= .1)
            {
                
            }
        }

    }
}
