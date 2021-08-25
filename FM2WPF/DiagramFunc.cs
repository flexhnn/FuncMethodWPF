using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FM2WPF
{
    class DiagramFunc
    {
        public WriteableBitmap bmp_z_source;
        public WriteableBitmap bmp_z;
        public WriteableBitmap bmp_h;
        public WriteableBitmap bmp_v;
        public WriteableBitmap bmp_d;

        public WriteableBitmap bmp_zg;
        public WriteableBitmap bmp_hg;
        public WriteableBitmap bmp_vg;
        public WriteableBitmap bmp_dg;

        Func_m f;
        public int size;
        public int dsize;
        Thickness zone_margin;

        public int[,] z, h, v, d;
        public int[,] z1, h1, v1, d1;

        public DiagramFunc(Func_m f, int size, Thickness zone_margin)
        {
            this.f = f;
            this.size = size;
            this.zone_margin = zone_margin;
            dsize = f.zona_size / size;
        }

        int[,] arzela_calc_zone(byte[] buffer)
        {
            byte[,] zone_arr = get_zone();

            int kvs = f.zona_size / size;

            int[,] res = new int[kvs, kvs];

            int sum = 0;
            int b1;
            int b2;

            for (int i = 0, ci = 0; i < f.zona_size - f.zona_size % size; i += size, ci++)
            {
                for (int j = 0, cj = 0; j < f.zona_size - f.zona_size % size; j += size, cj++)
                {

                    for (int l = 0; l < size - 1; l++)
                    {
                        for (int k = 0; k < size - 1; k++)
                        {
                            b1 = Func_m.abs(zone_arr[i + l + 1, j + k] - zone_arr[i + l, j + k]);
                            b2 = Func_m.abs(zone_arr[i + l, j + k + 1] - zone_arr[i + l, j + k]);
                            sum = sum + b1 + b2;
                        }
                    }

                    res[ci, cj] = sum;
                    sum = 0;
                }
            }

            int ti = 0;
            for (int i = 0; i < f.zona_size; i++)
            {
                for (int j = 0; j < f.zona_size; j++)
                {
                    buffer[ti++] = zone_arr[i, j];
                    buffer[ti++] = zone_arr[i, j];
                    buffer[ti++] = zone_arr[i, j];
                }
            }

            return res;
        }

        int[,] arzela_calc_h()
        {
            byte[,] zone_arr = get_zone_h();

            int kvs = f.zona_size / size;

            int[,] res = new int[kvs, kvs];

            int sum = 0;
            int b1;
            int b2;

            for (int i = 0, ci = 0; i < f.zona_size - f.zona_size % size; i += size, ci++)
            {
                for (int j = 0, cj = 0; j < f.zona_size - f.zona_size % size; j += size, cj++)
                {

                    for (int l = 0; l < size - 1; l++)
                    {
                        for (int k = 0; k < size - 1; k++)
                        {
                            b1 = Func_m.abs(zone_arr[i + l + 1, j + k] - zone_arr[i + l, j + k]);
                            b2 = Func_m.abs(zone_arr[i + l, j + k + 1] - zone_arr[i + l, j + k]);
                            sum = sum + b1 + b2;
                        }
                    }

                    res[ci, cj] = sum;
                    sum = 0;
                }
            }

            return res;
        }

        int[,] arzela_calc_v()
        {
            byte[,] zone_arr = get_zone_v();

            int kvs = f.zona_size / size;

            int[,] res = new int[kvs, kvs];

            int sum = 0;
            int b1;
            int b2;

            for (int i = 0, ci = 0; i < f.zona_size - f.zona_size % size; i += size, ci++)
            {
                for (int j = 0, cj = 0; j < f.zona_size - f.zona_size % size; j += size, cj++)
                {

                    for (int l = 0; l < size - 1; l++)
                    {
                        for (int k = 0; k < size - 1; k++)
                        {
                            b1 = Func_m.abs(zone_arr[i + l + 1, j + k] - zone_arr[i + l, j + k]);
                            b2 = Func_m.abs(zone_arr[i + l, j + k + 1] - zone_arr[i + l, j + k]);
                            sum = sum + b1 + b2;
                        }
                    }

                    res[ci, cj] = sum;
                    sum = 0;
                }
            }

            return res;
        }

        int[,] arzela_calc_d()
        {
            byte[,] zone_arr = get_zone_d();

            int kvs = f.zona_size / size;

            int[,] res = new int[kvs, kvs];

            int sum = 0;
            int b1;
            int b2;

            for (int i = 0, ci = 0; i < f.zona_size - f.zona_size % size; i += size, ci++)
            {
                for (int j = 0, cj = 0; j < f.zona_size - f.zona_size % size; j += size, cj++)
                {

                    for (int l = 0; l < size - 1; l++)
                    {
                        for (int k = 0; k < size - 1; k++)
                        {
                            b1 = Func_m.abs(zone_arr[i + l + 1, j + k] - zone_arr[i + l, j + k]);
                            b2 = Func_m.abs(zone_arr[i + l, j + k + 1] - zone_arr[i + l, j + k]);
                            sum = sum + b1 + b2;
                        }
                    }

                    res[ci, cj] = sum;
                    sum = 0;
                }
            }

            return res;
        }

        byte[,] get_zone()
        {
            byte[] mas = new byte[f.zona_size * f.zona_size * 3];
            Int32Rect z_rect = new Int32Rect((int)(zone_margin.Left), (int)zone_margin.Top, f.zona_size, f.zona_size);
            f.bmp_main.CopyPixels(z_rect, mas, f.zona_size * 3, 0);
            byte[,] rs = new byte[f.zona_size, f.zona_size];
            int k = 0;
            for (int i = 0; i < f.zona_size; i++)
            {
                for (int j = 0; j < f.zona_size; j++)
                {
                    rs[i, j] = mas[k];
                    k += 3;
                }
            }
            return rs;
        }

        byte[,] get_zone_h()
        {
            byte[] mas = new byte[f.zona_size * f.zona_size * 3];
            Int32Rect z_rect = new Int32Rect(0, 0, f.zona_size, f.zona_size);
            f.bmp_result_h.CopyPixels(z_rect, mas, f.zona_size * 3, 0);
            byte[,] rs = new byte[f.zona_size, f.zona_size];
            int k = 0;
            for (int i = 0; i < f.zona_size; i++)
            {
                for (int j = 0; j < f.zona_size; j++)
                {
                    rs[i, j] = mas[k];
                    k += 3;
                }
            }
            return rs;
        }

        byte[,] get_zone_v()
        {
            byte[] mas = new byte[f.zona_size * f.zona_size * 3];
            Int32Rect z_rect = new Int32Rect(0, 0, f.zona_size, f.zona_size);
            f.bmp_result_v.CopyPixels(z_rect, mas, f.zona_size * 3, 0);
            byte[,] rs = new byte[f.zona_size, f.zona_size];
            int k = 0;
            for (int i = 0; i < f.zona_size; i++)
            {
                for (int j = 0; j < f.zona_size; j++)
                {
                    rs[i, j] = mas[k];
                    k += 3;
                }
            }
            return rs;
        }

        byte[,] get_zone_d()
        {
            byte[] mas = new byte[f.zona_size * f.zona_size * 3];
            Int32Rect z_rect = new Int32Rect(0, 0, f.zona_size, f.zona_size);
            f.bmp_result_d.CopyPixels(z_rect, mas, f.zona_size * 3, 0);
            byte[,] rs = new byte[f.zona_size, f.zona_size];
            int k = 0;
            for (int i = 0; i < f.zona_size; i++)
            {
                for (int j = 0; j < f.zona_size; j++)
                {
                    rs[i, j] = mas[k];
                    k += 3;
                }
            }
            return rs;
        }

        byte[,] get_zone_f(WriteableBitmap bmp, Thickness th = new Thickness())
        {
            byte[] mas = new byte[f.zona_size * f.zona_size * 3];
            Int32Rect z_rect = new Int32Rect((int)th.Left, (int)th.Right, f.zona_size, f.zona_size);
            bmp.CopyPixels(z_rect, mas, f.zona_size * 3, 0);
            byte[,] rs = new byte[f.zona_size, f.zona_size];
            int k = 0;
            for (int i = 0; i < f.zona_size; i++)
            {
                for (int j = 0; j < f.zona_size; j++)
                {
                    rs[i, j] = mas[k];
                    k += 3;
                }
            }
            return rs;
        }

        public int hfull = 0, vfull = 0, dfull = 0, zfull = 0;

        public void gen_plains()
        {
            byte[] buf = new byte[f.zona_size * f.zona_size * 3];
            z1 = arzela_calc_zone(buf);
            h1 = arzela_calc_h();
            v1 = arzela_calc_v();
            d1 = arzela_calc_d();

            z = new int[dsize, dsize];
            h = new int[dsize, dsize];
            v = new int[dsize, dsize];
            d = new int[dsize, dsize];

            int maxH, maxV, maxD, maxZ;

            int len = dsize * dsize * 100 * 3;

            maxH = Int32.MinValue;
            maxV = maxH;
            maxD = maxH;
            maxZ = maxH;

            for (int i = 0; i < dsize; i++)
            {
                for (int j = 0; j < dsize; j++)
                {
                    if (maxH < h1[i, j]) maxH = h1[i, j];
                    if (maxV < v1[i, j]) maxV = v1[i, j];
                    if (maxD < d1[i, j]) maxD = d1[i, j];
                    if (maxZ < z1[i, j]) maxZ = z1[i, j];

                    z[i, j] = z1[i, j];
                    h[i, j] = h1[i, j];
                    v[i, j] = v1[i, j];
                    d[i, j] = d1[i, j];

                    hfull += h[i, j];
                    vfull += v[i, j];
                    dfull += d[i, j];
                    zfull += z[i, j];
                }
            }

            byte[] ar = new byte[len];
            byte[] ar1 = new byte[len];
            byte[] ar2 = new byte[len];
            byte[] ar3 = new byte[len];
            int ti = 0;

            for (int i = 0; i < dsize; i++)
            {
                for (int j = 0; j < dsize; j++)
                {
                    z[i, j] = z[i, j] * 255 / maxZ;
                    h[i, j] = h[i, j] * 255 / maxH;
                    v[i, j] = v[i, j] * 255 / maxV;
                    d[i, j] = d[i, j] * 255 / maxD;
                }
            }

            for (int i = 0; i < dsize; i++)
            {
                for (int ll = 0; ll < 10; ll++)
                {
                    for (int j = 0; j < dsize; j++)
                    {


                        for (int k = 0; k < 10; k++)
                        {
                            ar[ti] = (byte)z[i, j];
                            ar1[ti] = (byte)h[i, j];
                            ar2[ti] = (byte)v[i, j];
                            ar3[ti++] = (byte)d[i, j];

                            ar[ti] = (byte)z[i, j];
                            ar1[ti] = (byte)h[i, j];
                            ar2[ti] = (byte)v[i, j];
                            ar3[ti++] = (byte)d[i, j];

                            ar[ti] = (byte)z[i, j];
                            ar1[ti] = (byte)h[i, j];
                            ar2[ti] = (byte)v[i, j];
                            ar3[ti++] = (byte)d[i, j];
                        }
                    }
                }
            }

            int wh = 10 * dsize;
            int stride = wh * 3;

            bmp_z_source = new WriteableBitmap(f.zona_size, f.zona_size, 96, 96, PixelFormats.Bgr24, null);
            Int32Rect zrct = new Int32Rect(0, 0, f.zona_size, f.zona_size);
            bmp_z_source.WritePixels(zrct, buf, f.zona_size * 3, 0);

            bmp_z = new WriteableBitmap(wh, wh, 96, 96, PixelFormats.Bgr24, null);
            Int32Rect rct = new Int32Rect(0, 0, wh, wh);
            bmp_z.WritePixels(rct, ar, stride, 0);

            bmp_h = new WriteableBitmap(wh, wh, 96, 96, PixelFormats.Bgr24, null);
            bmp_h.WritePixels(rct, ar1, stride, 0);

            bmp_v = new WriteableBitmap(wh, wh, 96, 96, PixelFormats.Bgr24, null);
            bmp_v.WritePixels(rct, ar2, stride, 0);

            bmp_d = new WriteableBitmap(wh, wh, 96, 96, PixelFormats.Bgr24, null);
            bmp_d.WritePixels(rct, ar3, stride, 0);
        }
    }
}
