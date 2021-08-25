using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FM2WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Charts> chrts;

        string path;
        OpenFileDialog ofd;
        FileStream fs;

        Func_m func_1;
        Func_m func_2;

        #region перемещение зоны

        bool is_dragged;

        int oldX, oldY;
        Thickness zone_margin;
        Rectangle rct;

        int oldX_2, oldY_2;
        Thickness zone_margin_2;
        Rectangle rct_2;

        #endregion

        short[] two_bytes; //Массивы для хранения данных файлов с расширением .mbv
        short[] two_bytes_2;

        byte[] bytes_offset_0; //Массивы данных для расширения .mbv с нулевым смещением
        byte[] bytes_offset_0_s;

        byte[] bytes_offset_1; //Массивы данных для расширения .mbv с единичным смещением вправо
        byte[] bytes_offset_1_s;

        byte[] bytes_offset_2; //Массивы данных для расширения .mbv с двойным смещением вправо
        byte[] bytes_offset_2_s;

        public MainWindow()
        {
            InitializeComponent();

            func_1 = new Func_m();
            func_2 = new Func_m();

            chrts = new List<Charts>();

            bt_open.Click += Bt_open_Click;
            bt_open2.Click += Bt_open_Click;

            bt_test3.Click += Bt_test3_Click;
            bt_test3_2.Click += Bt_test3_Click;

            img_grid.MouseDown += Img_grid_MouseDown;
            img_grid.MouseUp += Img_grid_MouseUp;
            img_grid.MouseMove += Img_grid_MouseMove;
            img_grid.MouseLeave += Img_grid_MouseLeave;

            img_grid_2.MouseDown += Img_grid_MouseDown;
            img_grid_2.MouseUp += Img_grid_MouseUp;
            img_grid_2.MouseMove += Img_grid_MouseMove;
            img_grid_2.MouseLeave += Img_grid_MouseLeave;

            slider_zone.ValueChanged += Slider_zone_ValueChanged;
            slider_window.ValueChanged += Slider_window_ValueChanged;

            slider_zone_2.ValueChanged += Slider_zone_ValueChanged;
            slider_window_2.ValueChanged += Slider_window_ValueChanged;

            cb_szone.Checked += Cb_szone_Checked;

            zone_margin = new Thickness(0, 0, 0, 0);
            zone_margin_2 = new Thickness(0, 0, 0, 0);

            ofd = new OpenFileDialog();
            ofd.Filter = "Image files(*.BMP;*.JPG;*GIF;*.PNG;*.MBV)|*.BMP;*.JPG;*.GIF;*.PNG;*.MBV";

            rb_offset_0.Checked += Rb_offset_Checked;
            rb_offset_1.Checked += Rb_offset_Checked;
            rb_offset_2.Checked += Rb_offset_Checked;

            rb_offset_0_s.Checked += Rb_offset_0_s_Checked;
            rb_offset_1_s.Checked += Rb_offset_0_s_Checked;
            rb_offset_2_s.Checked += Rb_offset_0_s_Checked;

            bt_charts.Click += Bt_charts_Click;
        }

        private void Rb_offset_0_s_Checked(object sender, RoutedEventArgs e)
        {
             Int32Rect rct = new Int32Rect(0, 0, func_2.bmp_main.PixelWidth, func_2.bmp_main.PixelHeight);
             if ((bool)rb_offset_0_s.IsChecked)
                func_2.bmp_main.WritePixels(rct, bytes_offset_0_s, func_2.bmp_main.PixelWidth * 3, 0);
             else if ((bool)rb_offset_1_s.IsChecked)
                func_2.bmp_main.WritePixels(rct, bytes_offset_1_s, func_2.bmp_main.PixelWidth * 3, 0);
             else
                func_2.bmp_main.WritePixels(rct, bytes_offset_2_s, func_2.bmp_main.PixelWidth * 3, 0);
        }

        private void Cb_szone_Checked(object sender, RoutedEventArgs e)
        {
            zone_margin = new Thickness(0, 0, 0, 0);
            zone_margin_2 = new Thickness(0, 0, 0, 0);

            rct.Margin = zone_margin;
            rct_2.Margin = zone_margin_2;
        }

        private void Bt_charts_Click(object sender, RoutedEventArgs e)
        {
            Charts ch = new Charts(func_1, func_2, 50, zone_margin, zone_margin_2);
            chrts.Add(ch);

            ch.draw_pc();
            
            ch.Show(); 
        }

        private void Rb_offset_Checked(object sender, RoutedEventArgs e)
        {
            Int32Rect rct = new Int32Rect(0, 0, func_1.bmp_main.PixelWidth, func_1.bmp_main.PixelHeight);
            if ((bool)rb_offset_0.IsChecked)
                func_1.bmp_main.WritePixels(rct, bytes_offset_0, func_1.bmp_main.PixelWidth * 3, 0);
            else if ((bool)rb_offset_1.IsChecked)
                func_1.bmp_main.WritePixels(rct, bytes_offset_1, func_1.bmp_main.PixelWidth * 3, 0);
            else
                func_1.bmp_main.WritePixels(rct, bytes_offset_2, func_1.bmp_main.PixelWidth * 3, 0);
        }

        private void Slider_window_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = sender as Slider;
            if (s.Uid.Equals("sw1"))
                func_1.w_size = (int)s.Value;

            else
                func_2.w_size = (int)s.Value;
        }

        private void Slider_zone_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = sender as Slider;
            if ((func_1.bmp_main == null && s.Uid.Equals("s1")) || func_2.bmp_main == null && s.Uid.Equals("s2"))
            {
                s.Value = 150; 
                return;
            }

            if ((bool)cb_szone.IsChecked)
            {
                if (zone_margin.Left + (int)s.Value > func_1.bmp_main.PixelWidth || zone_margin.Top + (int)s.Value > func_1.bmp_main.PixelHeight
                    || zone_margin_2.Left + (int)s.Value > func_2.bmp_main.PixelWidth || zone_margin_2.Top + (int)s.Value > func_2.bmp_main.PixelHeight)
                {
                    s.Value = func_1.zona_size;
                }
                else
                {
                    func_1.zona_size = (int)s.Value;
                    func_2.zona_size = (int)s.Value;

                    img_grid.Children.Remove(rct);
                    img_grid_2.Children.Remove(rct_2);

                    rct = new Rectangle
                    {
                        Width = func_1.zona_size,  //Установка
                        Height = func_1.zona_size, //размеров

                        HorizontalAlignment = HorizontalAlignment.Left, //Установка
                        VerticalAlignment = VerticalAlignment.Top,      //выравнивания

                        Margin = zone_margin,   //Установка позиции

                        Stroke = Brushes.Cyan //Установка цвета
                    };

                    rct_2 = new Rectangle
                    {
                        Width = func_2.zona_size,  //Установка
                        Height = func_2.zona_size, //размеров

                        HorizontalAlignment = HorizontalAlignment.Left, //Установка
                        VerticalAlignment = VerticalAlignment.Top,      //выравнивания

                        Margin = zone_margin_2,   //Установка позиции

                        Stroke = Brushes.Cyan //Установка цвета
                    };

                    img_grid.Children.Add(rct); //Добавление
                    img_grid_2.Children.Add(rct_2); //Добавление
                }
            }
            else
            {
                if (s.Uid.Equals("s1"))
                {
                    if (zone_margin.Left + (int)slider_zone.Value > func_1.bmp_main.PixelWidth || zone_margin.Top + (int)slider_zone.Value > func_1.bmp_main.PixelHeight)
                    {
                        slider_zone.Value = func_1.zona_size;
                    }
                    else
                    {
                        func_1.zona_size = (int)slider_zone.Value;

                        img_grid.Children.Remove(rct);

                        rct = new Rectangle
                        {
                            Width = func_1.zona_size,  //Установка
                            Height = func_1.zona_size, //размеров

                            HorizontalAlignment = HorizontalAlignment.Left, //Установка
                            VerticalAlignment = VerticalAlignment.Top,      //выравнивания

                            Margin = zone_margin,   //Установка позиции

                            Stroke = Brushes.Cyan //Установка цвета
                        };

                        img_grid.Children.Add(rct); //Добавление
                    }
                }
                else
                {
                    if (zone_margin_2.Left + (int)slider_zone_2.Value > func_2.bmp_main.PixelWidth || zone_margin_2.Top + (int)slider_zone_2.Value > func_2.bmp_main.PixelHeight)
                    {
                        slider_zone_2.Value = func_2.zona_size;
                    }
                    else
                    {
                        func_2.zona_size = (int)slider_zone_2.Value;

                        img_grid_2.Children.Remove(rct_2);

                        rct_2 = new Rectangle
                        {
                            Width = func_2.zona_size,  //Установка
                            Height = func_2.zona_size, //размеров

                            HorizontalAlignment = HorizontalAlignment.Left, //Установка
                            VerticalAlignment = VerticalAlignment.Top,      //выравнивания

                            Margin = zone_margin_2,   //Установка позиции

                            Stroke = Brushes.Cyan //Установка цвета
                        };

                        img_grid_2.Children.Add(rct_2); //Добавление
                    }
                }
            }   
        }

        private void Img_grid_MouseLeave(object sender, MouseEventArgs e)
        {
            is_dragged = false;
        }

        private void Img_grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (is_dragged)
            {
                Grid g = sender as Grid;
                Point p = e.GetPosition(g);

                if ((bool)cb_szone.IsChecked)
                {
                    int offsetX = (int)p.X - oldX;
                    int offsetY = (int)p.Y - oldY;

                    int rsltX = (int)(zone_margin.Left + offsetX);
                    int rsltY = (int)(zone_margin.Top + offsetY);

                    if (rsltX >= 0 && rsltX + func_1.zona_size <= func_1.bmp_main.PixelWidth && rsltX + func_2.zona_size <= func_2.bmp_main.PixelWidth)
                    {
                        zone_margin.Left = rsltX;
                        zone_margin_2.Left = rsltX;
                    }
                    if (rsltY >= 0 && rsltY + func_1.zona_size <= func_1.bmp_main.PixelHeight && rsltY + func_2.zona_size <= func_2.bmp_main.PixelHeight)
                    {
                        zone_margin.Top = rsltY;
                        zone_margin_2.Top = rsltY;
                    }

                    oldX = (int)(p.X);
                    oldY = (int)(p.Y);
                    oldX_2 = oldX;
                    oldY_2 = oldY;

                    rct.Margin = zone_margin;
                    rct_2.Margin = zone_margin;
                }
                else
                {
                    if (g.Uid.Equals("g1"))
                    {
                        int offsetX = (int)p.X - oldX;
                        int offsetY = (int)p.Y - oldY;

                        int rsltX = (int)(zone_margin.Left + offsetX);
                        int rsltY = (int)(zone_margin.Top + offsetY);

                        if (rsltX >= 0 && rsltX + func_1.zona_size <= func_1.bmp_main.PixelWidth)
                        {
                            zone_margin.Left = rsltX;
                        }
                        if (rsltY >= 0 && rsltY + func_1.zona_size <= func_1.bmp_main.PixelHeight)
                        {
                            zone_margin.Top = rsltY;
                        }

                        oldX = (int)(p.X);
                        oldY = (int)(p.Y);

                        rct.Margin = zone_margin;
                    }
                    else
                    {
                        int offsetX = (int)p.X - oldX_2;
                        int offsetY = (int)p.Y - oldY_2;

                        int rsltX = (int)(zone_margin_2.Left + offsetX);
                        int rsltY = (int)(zone_margin_2.Top + offsetY);

                        if (rsltX >= 0 && rsltX + func_2.zona_size <= func_2.bmp_main.PixelWidth)
                        {
                            zone_margin_2.Left = rsltX;
                        }
                        if (rsltY >= 0 && rsltY + func_2.zona_size <= func_2.bmp_main.PixelHeight)
                        {
                            zone_margin_2.Top = rsltY;
                        }

                        oldX_2 = (int)(p.X);
                        oldY_2 = (int)(p.Y);

                        rct_2.Margin = zone_margin_2;
                    }
                }
            }
        }

        private void Img_grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            is_dragged = false;
        }

        private void Img_grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;
            Point p = e.GetPosition(g);

            if (g.Uid.Equals("g1"))
            {
                if (p.X > zone_margin.Left && p.X < zone_margin.Left + func_1.zona_size)
                {
                    if (p.Y > zone_margin.Top && p.Y < zone_margin.Top + func_1.zona_size)
                    {
                        oldX = (int)p.X;
                        oldY = (int)p.Y;
                        is_dragged = true;
                    }
                }
            }
            else
            {
                if (p.X > zone_margin_2.Left && p.X < zone_margin_2.Left + func_2.zona_size)
                {
                    if (p.Y > zone_margin_2.Top && p.Y < zone_margin_2.Top + func_2.zona_size)
                    {
                        oldX_2 = (int)p.X;
                        oldY_2 = (int)p.Y;
                        is_dragged = true;
                    }
                }
            }
        }

        private void Bt_test3_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (func_1.bmp_main != null && b.Uid.Equals("bs1"))
            {
                if ((bool)rb_fb1.IsChecked)
                {
                    func_1.base_func_1(zone_margin);
                }
                    
                else
                {
                    func_1.base_func_2(zone_margin);
                }

                img_h.Source = func_1.bmp_result_h;
                img_v.Source = func_1.bmp_result_v;
                img_d.Source = func_1.bmp_result_d;

                bt_charts.IsEnabled = true;
            }
            else if (func_2.bmp_main != null && b.Uid.Equals("bs2"))
            {
                if ((bool)rb_fb1.IsChecked)
                {
                    func_2.base_func_1(zone_margin_2);
                }  
                else
                {
                    func_2.base_func_2(zone_margin_2);
                }

                img_h_2.Source = func_2.bmp_result_h;
                img_v_2.Source = func_2.bmp_result_v;
                img_d_2.Source = func_2.bmp_result_d;

                bt_charts.IsEnabled = true;
            }
        }

        private void Bt_open_Click(object sender, RoutedEventArgs e)
        {
            if (ofd.ShowDialog() == true)
            {
                Button b = sender as Button;
                try
                {
                    path = ofd.FileName;

                    string[] strs = path.Split('.');
                    if (strs[strs.Length - 1].Equals("mbv") || strs[strs.Length - 1].Equals("MBV"))
                    {
                        fs = File.OpenRead(path);
                        byte[] wh = new byte[4];
                        fs.Read(wh, 0, wh.Length);

                        int width = wh[1];
                        width <<= 8;
                        width |= wh[0];

                        int height = wh[3];
                        height <<= 8;
                        height |= wh[2];

                        byte[] bts = new byte[fs.Length - 4];
                        fs.Read(bts, 0, bts.Length);
                        fs.Close();

                        short[] buf_tb = new short[bts.Length / 2];

                        short sh_buf = 0;

                        int k = 0;

                        for (int i = 0; i < bts.Length; i+=2)
                        {
                            sh_buf |= bts[i + 1];
                            sh_buf <<= 8;
                            sh_buf |= bts[i];
                            sh_buf &= 0b_0000_0011_1111_1111;
                            buf_tb[k] = sh_buf;
                            sh_buf = 0;
                            ++k;
                        }

                        byte[] buf_bts_of0 = new byte[bts.Length / 2 * 3];
                        byte[] buf_bts_of1 = new byte[buf_bts_of0.Length];
                        byte[] buf_bts_of2 = new byte[buf_bts_of0.Length];

                        int d = 0;
                        for (int i = 0; i < buf_tb.Length; i++)
                        {
                            buf_bts_of0[d] = (byte)buf_tb[i];
                            buf_bts_of0[d + 1] = buf_bts_of0[d];
                            buf_bts_of0[d + 2] = buf_bts_of0[d];

                            buf_bts_of1[d] = (byte)(buf_tb[i] >> 1);
                            buf_bts_of1[d + 1] = buf_bts_of1[d];
                            buf_bts_of1[d + 2] = buf_bts_of1[d];

                            buf_bts_of2[d] = (byte)(buf_tb[i] >> 2);
                            buf_bts_of2[d + 1] = buf_bts_of2[d];
                            buf_bts_of2[d + 2] = buf_bts_of2[d];

                            d += 3;
                        }

                        WriteableBitmap buf_bmp = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);
                        Int32Rect rect = new Int32Rect(0, 0, width, height);
                        buf_bmp.WritePixels(rect, buf_bts_of0, width * 3, 0);

                        if (b.Uid.Equals("1"))
                        {
                            img_bw.Width = width;
                            img_bw.Height = height;

                            two_bytes = buf_tb;
                            bytes_offset_0 = buf_bts_of0;
                            bytes_offset_1 = buf_bts_of1;
                            bytes_offset_2 = buf_bts_of2;

                            func_1.load_source_mbv(buf_bmp);
                            func_1.r_date = two_bytes; //**

                            gb_mbv_offset.IsEnabled = true;
                            rb_offset_0.IsChecked = true;
                        }
                        else
                        {
                            img_bw_2.Width = width;
                            img_bw_2.Height = height;

                            two_bytes_2 = buf_tb;
                            bytes_offset_0_s = buf_bts_of0;
                            bytes_offset_1_s = buf_bts_of1;
                            bytes_offset_2_s = buf_bts_of2;

                            func_2.load_source_mbv(buf_bmp);
                            func_2.r_date = two_bytes_2; //**

                            gb_mbv_offset_s.IsEnabled = true;
                            rb_offset_0_s.IsChecked = true;
                        }
                    }
                    else
                    {
                        WriteableBitmap bmp_source_buf = new WriteableBitmap(new BitmapImage(new Uri(path)));

                        if (bmp_source_buf.PixelWidth < 150 || bmp_source_buf.PixelHeight < 150)
                        {
                            MessageBox.Show("Размер изображения должен быть не менее 150x150", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            if (b.Uid.Equals("1"))
                            {
                                func_1.bmp_source = bmp_source_buf;

                                img_bw.Width = func_1.bmp_source.PixelWidth;
                                img_bw.Height = func_1.bmp_source.PixelHeight;
                            }
                            else
                            {
                                func_2.bmp_source = bmp_source_buf;

                                img_bw_2.Width = func_2.bmp_source.PixelWidth;
                                img_bw_2.Height = func_2.bmp_source.PixelHeight;
                            }

                            gb_mbv_offset_s.IsEnabled = false;
                        }
                    }

                    if (b.Uid.Equals("1"))
                    {
                        img_bw.Source = func_1.bmp_main;
                        load_rect();
                    }
                    else
                    {
                        img_bw_2.Source = func_2.bmp_main;
                        load_rect2();
                    }

                    if (func_1.bmp_main != null && func_2.bmp_main != null) cb_szone.IsEnabled = true;
                }
                catch
                {
                    MessageBox.Show("Ошибка открытия файла", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (fs != null) fs.Close();
                }
            }
        }

        void load_rect()
        {
            if (rct != null) img_grid.Children.Remove(rct);
            rct = new Rectangle();  //Инициализация

            rct.Width = func_1.zona_size;  //Установка
            rct.Height = func_1.zona_size; //размеров

            rct.HorizontalAlignment = HorizontalAlignment.Left; //Установка
            rct.VerticalAlignment = VerticalAlignment.Top;      //выравнивания

            zone_margin.Left = 0;
            zone_margin.Top = 0;
            
            rct.Margin = zone_margin;   //Установка позиции

            rct.Stroke = Brushes.Cyan; //Установка цвета

            img_grid.Children.Add(rct); //Добавление
        }

        void load_rect2()
        {
            if (rct_2 != null) img_grid_2.Children.Remove(rct_2);
            rct_2 = new Rectangle();  //Инициализация

            rct_2.Width = func_2.zona_size;  //Установка
            rct_2.Height = func_2.zona_size; //размеров

            rct_2.HorizontalAlignment = HorizontalAlignment.Left; //Установка
            rct_2.VerticalAlignment = VerticalAlignment.Top;      //выравнивания

            zone_margin_2.Left = 0;
            zone_margin_2.Top = 0;

            rct_2.Margin = zone_margin_2;   //Установка позиции

            rct_2.Stroke = Brushes.Cyan; //Установка цвета

            img_grid_2.Children.Add(rct_2); //Добавление
        }
    }
}
