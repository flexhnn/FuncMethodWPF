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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace FM2WPF
{
    /// <summary>
    /// Логика взаимодействия для Charts.xaml
    /// </summary>
    public partial class Charts : Window
    {
        DiagramFunc df;
        DiagramFunc df2;

        Func_m f;
        Func_m f2;

        Thickness zone_margin;
        Thickness zone_margin2;

        bool dragged = false;
        Point oldXY;

        public Charts(Func_m f, Func_m f2, int size, Thickness zone_margin, Thickness zone_margin2)
        {
            InitializeComponent();
            df = new DiagramFunc(f, size, zone_margin);
            df2 = new DiagramFunc(f2, size, zone_margin2);

            this.f = f;
            this.f2 = f2;
            this.zone_margin = zone_margin;
            this.zone_margin2 = zone_margin2;

            vp_z1.MouseDown += Vp_MouseDown;
            vp_h1.MouseDown += Vp_MouseDown;
            vp_v1.MouseDown += Vp_MouseDown;
            vp_d1.MouseDown += Vp_MouseDown;

            vp_z1.MouseUp += Vp_MouseUp;
            vp_h1.MouseUp += Vp_MouseUp;
            vp_v1.MouseUp += Vp_MouseUp;
            vp_d1.MouseUp += Vp_MouseUp;

            vp_z1.MouseLeave += Vp_MouseLeave;
            vp_h1.MouseLeave += Vp_MouseLeave;
            vp_v1.MouseLeave += Vp_MouseLeave;
            vp_d1.MouseLeave += Vp_MouseLeave;

            vp_z1.MouseMove += Vp_MouseMove;
            vp_h1.MouseMove += Vp_MouseMove;
            vp_v1.MouseMove += Vp_MouseMove;
            vp_d1.MouseMove += Vp_MouseMove;




            vp_z2.MouseDown += Vp_MouseDown;
            vp_h2.MouseDown += Vp_MouseDown;
            vp_v2.MouseDown += Vp_MouseDown;
            vp_d2.MouseDown += Vp_MouseDown;

            vp_z2.MouseUp += Vp_MouseUp;
            vp_h2.MouseUp += Vp_MouseUp;
            vp_v2.MouseUp += Vp_MouseUp;
            vp_d2.MouseUp += Vp_MouseUp;

            vp_z2.MouseLeave += Vp_MouseLeave;
            vp_h2.MouseLeave += Vp_MouseLeave;
            vp_v2.MouseLeave += Vp_MouseLeave;
            vp_d2.MouseLeave += Vp_MouseLeave;

            vp_z2.MouseMove += Vp_MouseMove;
            vp_h2.MouseMove += Vp_MouseMove;
            vp_v2.MouseMove += Vp_MouseMove;
            vp_d2.MouseMove += Vp_MouseMove;


            vp_z1.MouseWheel += Vp_MouseWheel;
            vp_h1.MouseWheel += Vp_MouseWheel;
            vp_v1.MouseWheel += Vp_MouseWheel;
            vp_d1.MouseWheel += Vp_MouseWheel;

            vp_z2.MouseWheel += Vp_MouseWheel;
            vp_h2.MouseWheel += Vp_MouseWheel;
            vp_v2.MouseWheel += Vp_MouseWheel;
            vp_d2.MouseWheel += Vp_MouseWheel;
        }

        private void Vp_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScaleTransform3D sc3 = null;

            Viewport3D vp = sender as Viewport3D;

            switch(vp.Name)
            {
                case "vp_z1":
                    sc3 = scale_z1;
                    break;
                case "vp_h1":
                    sc3 = scale_h1;
                    break;
                case "vp_v1":
                    sc3 = scale_v1;
                    break;
                case "vp_d1":
                    sc3 = scale_d1;
                    break;
                case "vp_z2":
                    sc3 = scale_z2;
                    break;                   
                case "vp_h2":
                    sc3 = scale_h2;
                    break;                   
                case "vp_v2":
                    sc3 = scale_v2;
                    break;                   
                case "vp_d2":
                    sc3 = scale_d2;
                    break;
            }

            if (e.Delta > 0)
            {
                sc3.ScaleX += 0.01;
                sc3.ScaleY += 0.01;
                sc3.ScaleZ += 0.01;
            }
            else
            {
                sc3.ScaleX -= 0.01;
                sc3.ScaleY -= 0.01;
                sc3.ScaleZ -= 0.01;
            }
        }

        private void Vp_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragged)
            {
                Viewport3D v3d = sender as Viewport3D;
                Point p = e.GetPosition(v3d);
                double offsetX = p.X - oldXY.X;
                double offsetY = p.Y - oldXY.Y;

                AxisAngleRotation3D angleY = null;
                AxisAngleRotation3D angleX = null;

                switch(v3d.Name)
                {
                    case "vp_z1":
                        angleX = rotateX_z1;
                        angleY = rotateY_z1;
                        break;
                    case "vp_h1":
                        angleX = rotateX_h1;
                        angleY = rotateY_h1;
                        break;
                    case "vp_v1":
                        angleX = rotateX_v1;
                        angleY = rotateY_v1;
                        break;
                    case "vp_d1":
                        angleX = rotateX_d1;
                        angleY = rotateY_d1;
                        break;
                    case "vp_z2":
                        angleX = rotateX_z2;     //!!!!!!!!!!!!!
                        angleY = rotateY_z2;     //!!!!!!!!!!!!!
                        break;                   //!!!!!!!!!!!!!
                    case "vp_h2":                //!!!!!!!!!!!!!
                        angleX = rotateX_h2;     //!!!!!!!!!!!!!
                        angleY = rotateY_h2;     //!!!!!!!!!!!!!
                        break;                   //!!!!!!!!!!!!!
                    case "vp_v2":                //!!!!!!!!!!!!!
                        angleX = rotateX_v2;     //!!!!!!!!!!!!!
                        angleY = rotateY_v2;     //!!!!!!!!!!!!!
                        break;                   //!!!!!!!!!!!!!
                    case "vp_d2":                //!!!!!!!!!!!!!
                        angleX = rotateX_d2;     //!!!!!!!!!!!!!
                        angleY = rotateY_d2;     //!!!!!!!!!!!!!
                        break;
                }

                if (abs(offsetX) > 0.5)
                {
                    if (offsetX > 0)
                    {
                        angleY.Angle += 0.6;
                    }
                    else
                    {
                        angleY.Angle -= 0.6;
                    }
                }


                if (abs(offsetY) > 0.5)
                {
                    if (offsetY > 0)
                    {

                        angleX.Angle += 0.6;
                    }
                    else
                    {
                        angleX.Angle -= 0.6;
                    }
                }


                oldXY.X = p.X;
                oldXY.Y = p.Y;
            }
        }

        double abs(double v)
        {
            if (v < 0) return -1 * v;
            return v;
        }

        private void Vp_MouseLeave(object sender, MouseEventArgs e)
        {
            //dragged = false;
        }

        private void Vp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dragged = false;
            Mouse.Capture(null);
        }

        private void Vp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(sender as Viewport3D);
            oldXY.X = p.X;
            oldXY.Y = p.Y;
            dragged = true;
            Mouse.Capture(sender as IInputElement);
        }

        public void draw_pc()
        {
            if (f.bmp_result_h != null)
            {
                img_hs.Source = f.bmp_result_h;
                img_vs.Source = f.bmp_result_v;
                img_ds.Source = f.bmp_result_d;

                draw_plains();
            }
            if (f2.bmp_result_h != null)
            {
                img_hs_2.Source = f2.bmp_result_h;
                img_vs_2.Source = f2.bmp_result_v;
                img_ds_2.Source = f2.bmp_result_d;

                draw_plains2();
            }
        }

        void draw_plains()
        {
            df.gen_plains();
            img_zd.Source = df.bmp_z;
            img_hd.Source = df.bmp_h;
            img_vd.Source = df.bmp_v;
            img_dd.Source = df.bmp_d;
            img_zs.Source = df.bmp_z_source;

            img_chrt_z.Source = df.bmp_z;
            img_chrt_h.Source = df.bmp_h;
            img_chrt_v.Source = df.bmp_v;
            img_chrt_d.Source = df.bmp_d;

            z_frag_val.Text = $"Исходный фрагмент: {df.zfull}";
            h_frag_val.Text = $"Горизонтальная пара: {df.hfull}";
            v_frag_val.Text = $"Вертикальная пара: {df.vfull}";
            d_frag_val.Text = $"Диагональная пара: {df.dfull}";

            g_source_1.Content = build3D(df.z);
            g_horizontal_1.Content = build3D(df.h);
            g_vertical_1.Content = build3D(df.v);
            g_diagonal_1.Content = build3D(df.d);
        }

        void draw_plains2()
        {
            df2.gen_plains();

            img_zd_2.Source = df2.bmp_z;
            img_hd_2.Source = df2.bmp_h;
            img_vd_2.Source = df2.bmp_v;
            img_dd_2.Source = df2.bmp_d;
            img_zs_2.Source = df2.bmp_z_source;

            img_chrt_z_s.Source = df2.bmp_z;
            img_chrt_h_s.Source = df2.bmp_h;
            img_chrt_v_s.Source = df2.bmp_v;
            img_chrt_d_s.Source = df2.bmp_d;

            z_frag_val_2.Text = $"Исходный фрагмент: {df2.zfull}";
            h_frag_val_2.Text = $"Горизонтальная пара: {df2.hfull}";
            v_frag_val_2.Text = $"Вертикальная пара: {df2.vfull}";
            d_frag_val_2.Text = $"Диагональная пара: {df2.dfull}";

            g_source_2.Content = build3D(df2.z);
            g_horizontal_2.Content = build3D(df2.h);
            g_vertical_2.Content = build3D(df2.v);
            g_diagonal_2.Content = build3D(df2.d);
        }

        Model3DGroup build3D(int[,] ms)
        {
            Model3DGroup model3DGroup = new Model3DGroup();

            float[,] bts = new float[ms.GetLength(0), ms.GetLength(1)];

            int k = 0;
            double dal = -0.5;
            for (int i = 0; i < ms.GetLength(0); i++)
            {
                double blz = -0.5;
                for (int j = 0; j < ms.GetLength(1); j++)
                {
                    float kb = (float)ms[i, j] / (float)byte.MaxValue;

                    MeshGeometry3D bf3 = new MeshGeometry3D();
                    bf3.Positions.Add(new Point3D(blz, -0.5, dal));  //0  Слева внизу
                    bf3.Positions.Add(new Point3D(blz, -0.5 + kb, dal));   //1  Слева вверху
                    bf3.Positions.Add(new Point3D(blz + 0.1, -0.5 + kb, dal));    //2  Справа вверху
                    bf3.Positions.Add(new Point3D(blz + 0.1, -0.5, dal));   //3  Справа внизу

                    bf3.Positions.Add(new Point3D(blz, -0.5, dal + 0.1));   //4  Слева внизу
                    bf3.Positions.Add(new Point3D(blz, -0.5 + kb, dal + 0.1));    //5  Слева вверху
                    bf3.Positions.Add(new Point3D(blz + 0.1, -0.5 + kb, dal + 0.1));     //6  Справа вверху
                    bf3.Positions.Add(new Point3D(blz + 0.1, -0.5, dal + 0.1));    //7  Справа внизу

                    bf3.TriangleIndices.Add(0); bf3.TriangleIndices.Add(1); bf3.TriangleIndices.Add(2); //Back
                    bf3.TriangleIndices.Add(0); bf3.TriangleIndices.Add(2); bf3.TriangleIndices.Add(3);

                    bf3.TriangleIndices.Add(4); bf3.TriangleIndices.Add(1); bf3.TriangleIndices.Add(0); //Left
                    bf3.TriangleIndices.Add(1); bf3.TriangleIndices.Add(4); bf3.TriangleIndices.Add(5);

                    bf3.TriangleIndices.Add(0); bf3.TriangleIndices.Add(3); bf3.TriangleIndices.Add(4); //Bottom
                    bf3.TriangleIndices.Add(7); bf3.TriangleIndices.Add(4); bf3.TriangleIndices.Add(3);

                    bf3.TriangleIndices.Add(5); bf3.TriangleIndices.Add(2); bf3.TriangleIndices.Add(1); //Top
                    bf3.TriangleIndices.Add(2); bf3.TriangleIndices.Add(5); bf3.TriangleIndices.Add(6);

                    bf3.TriangleIndices.Add(6); bf3.TriangleIndices.Add(3); bf3.TriangleIndices.Add(2); //Right
                    bf3.TriangleIndices.Add(3); bf3.TriangleIndices.Add(6); bf3.TriangleIndices.Add(7);

                    bf3.TriangleIndices.Add(7); bf3.TriangleIndices.Add(6); bf3.TriangleIndices.Add(5); //Front
                    bf3.TriangleIndices.Add(4); bf3.TriangleIndices.Add(7); bf3.TriangleIndices.Add(5);

                    GeometryModel3D bmd3 = new GeometryModel3D();
                    bmd3.Geometry = bf3;
                    bmd3.Material = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb((byte)ms[i,j], (byte)ms[i,j], (byte)ms[i,j])));
                    model3DGroup.Children.Add(bmd3);

                    blz += 0.1;
                    ++k;
                }
                dal += 0.1;
            }

            return model3DGroup;
        }
    }
}
