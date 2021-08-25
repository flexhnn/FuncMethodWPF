using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FM2WPF
{
    public class Func_m
    {
        WriteableBitmap _bmp_source;
        public WriteableBitmap bmp_source
        {
            get { return _bmp_source; }
            set
            {
                _bmp_source = value;
                if (_bmp_source.PixelWidth < _zona_size || _bmp_source.PixelHeight < _zona_size)
                    _zona_size = _bmp_source.PixelWidth < _bmp_source.PixelHeight ? _bmp_source.PixelWidth : _bmp_source.PixelHeight;

                createBmpMain();
            }
        }//Исходное цветное изображение

        public WriteableBitmap bmp_main { private set; get; }       //Изображение в оттенках серого

        public WriteableBitmap bmp_result_h { private set; get; }   //Горизонтальная свертка
        public WriteableBitmap bmp_result_v { private set; get; }   //Вертикальная свертка
        public WriteableBitmap bmp_result_d { private set; get; }   //Диагональная свертка

        public int type
        {
            get { return _type; }
            set
            {
                _type = value;
                gen_kxy(_type);
            }
        }

        public int zona_size
        {
            get { return _zona_size; }
            set
            {
                _zona_size = value;
                bzone = _zona_size + (_w_size - 1) + w_sizeb;
            }
        }

        public int w_size 
        {
            get { return _w_size; }
            set
            {
                _w_size = value;
                w_sizeb = _w_size >> 1;
                bzone = _zona_size + (_w_size - 1) + w_sizeb;
                gen_kxy(_type);
            }
        }

        int _w_size = 8;
        int _zona_size = 150;
        int _type = 0;

        float[,] kxy;

        int bzone; //Размер зоны анализа с учетом выхода окон за пределы исходной зоны
        int w_sizeb; //Размер окна пополам

        public short[] r_date;

        public Func_m(WriteableBitmap bmp_source) : this()
        {
            _bmp_source = bmp_source;

            createBmpMain();
        }
        
        public Func_m()
        {
            w_sizeb = _w_size >> 1;  //Размер половины окна
            bzone = _zona_size + (_w_size - 1) + w_sizeb; //Размер зоны анализа с учетом выхода окон
            gen_kxy(_type);
        }

        void createBmpMain()
        {
            bmp_main = new WriteableBitmap(_bmp_source.PixelWidth, _bmp_source.PixelHeight, 96, 96, PixelFormats.Bgr24, null);

            byte[] bts = new byte[_bmp_source.PixelWidth * _bmp_source.PixelHeight * 4];
            _bmp_source.CopyPixels(bts, _bmp_source.PixelWidth * _bmp_source.Format.BitsPerPixel / 8, 0);
            byte[] new_bytes = new byte[bmp_main.PixelWidth * bmp_main.PixelHeight * 3];

            for (int i = 0, j = 0; i < bts.Length; i += 4, j += 3)
            {
                byte t = (byte)(bts[i] * 0.0722f + bts[i + 1] * 0.7152f + bts[i + 2] * 0.2126f);
                new_bytes[j] = t;
                new_bytes[j + 1] = t;
                new_bytes[j + 2] = t;
            }

            Int32Rect rect = new Int32Rect(0, 0, bmp_main.PixelWidth, bmp_main.PixelHeight);
            bmp_main.WritePixels(rect, new_bytes, bmp_main.PixelWidth * 3, 0);

            short[] shrt = new short[bmp_main.PixelWidth * bmp_main.PixelHeight];
            for (int i = 0, k = 0; i < shrt.Length; i++, k+=3)
            {
                shrt[i] = new_bytes[k];
            }
            r_date = shrt;
        }

        public void load_source_mbv(WriteableBitmap bmp)
        {
            bmp_main = bmp;
            if (bmp_main.PixelWidth < _zona_size || bmp_main.PixelHeight < _zona_size)
                _zona_size = bmp_main.PixelWidth < bmp_main.PixelHeight ? bmp_main.PixelWidth : bmp_main.PixelHeight;
        }

        void gen_kxy(int type)
        {
            //Инициализация массива, w_size - размер окна анализа
            kxy = new float[_w_size,_w_size];
            //Функция веса Kx
            float[] ves_1 = new float[_w_size];

            //Вычисление Kx и Ky
            for (int i = 0; i < w_sizeb; i++)
                ves_1[i] = 0.5f + i;

            for (int i = _w_size - 1, l = 0; i >= w_sizeb; i--, l++)
                ves_1[i] = 0.5f + l;

            if (type != 0) //Если нужна производная по Х
            {
                for (int i = _w_size - 1, l = 0; i >= w_sizeb; i--, l++)
                    ves_1[i] = -ves_1[i];

                if (type == 1)
                {
                    for (int i = 0; i < _w_size; i++)
                        for (int j = 0; j < _w_size; j++)
                            kxy[i, j] = ves_1[j];
                }
                else
                {
                    for (int i = 0; i < _w_size; i++)
                        for (int j = 0; j < _w_size; j++)
                            kxy[i, j] = ves_1[i];
                }

                return;
            }

            //Заполнение массива-ядра
            for (int i = 0; i < _w_size; i++)
                for (int j = 0; j < _w_size; j++)
                    kxy[i, j] = ves_1[i] * ves_1[j];
        }

        double[,] main_func(Thickness zone_margin)
        {
            //mas - получения фрагмента изображения
            short[,] mas = get_zone_data_ex(zone_margin);
            //dzone - размер массива основных функционалов
            //_zona_size - исходный размер фрагмента
            //_w_size - размер окна анализа
            int dzone = _zona_size + _w_size;
            double[,] res = new double[dzone, dzone];

            //sum - хранение промежуточного результата
            double sum = 0;

            //В цикле по массиву исходного фрагмента
            for (int i = 0; i < dzone; i++)       
            {                                     
                for (int j = 0; j < dzone; j++)   
                {
                    //В цикле по окну
                    for (int l = 0; l < _w_size; l++)    
                    {                                    
                        for (int y = 0; y < _w_size; y++)
                        {
                            //Расчет и суммирование очередного элемента 
                            //окна на соответствующий элемент массива ядра kxy
                            sum += (mas[i + l, j + y]) * kxy[l, y];
                        }
                    }

                    //Результат записывается в результирующий массив
                    res[i, j] = sum;
                    sum = 0;
                }
            }

            return res;
        }

        public void base_func_2(Thickness zone_margin)
        {
            int dzone = _zona_size + _w_size; //Размер зоны + размер окна пополам
            double[,] res = main_func(zone_margin);

            double[,] resh1 = new double[_zona_size, _zona_size]; //Массив для горизонтальной пары окон
            double[,] resh2 = new double[_zona_size, _zona_size]; //Массив для вертикальной пары окон
            double[,] resh3 = new double[_zona_size, _zona_size]; //Массив для диагональной пары окон

            for (int i = 0; i < _zona_size; i++) //В цикле по размеру новых массивов
            {
                for (int j = 0; j < _zona_size; j++)
                {
                    resh1[i, j] = res[i, j + w_sizeb] - res[i, j];       //Вычисляем числитель горизонтальной пары окон
                    resh2[i, j] = res[i + w_sizeb, j] - res[i, j];      //Вычисляем числитель вертикальной пары окон
                    resh3[i, j] = res[i + w_sizeb, j + w_sizeb] - res[i, j];  //Вычисляем числитель диагональной пары окон
                }
            }

            double znam_s;

            for (int i = w_sizeb, k = 0; i < dzone - w_sizeb; i++, k++)
            {
                for (int j = w_sizeb, l = 0; j < dzone - w_sizeb; j++, l++)
                {
                    //************************************************************
                    znam_s = res[i, j] - res[i, j - w_sizeb];
                    znam_s += res[i, j + w_sizeb] - res[i, j];

                    znam_s += res[i - w_sizeb, j] - res[i - w_sizeb, j - w_sizeb];
                    znam_s += res[i - w_sizeb, j + w_sizeb] - res[i - w_sizeb, j];

                    znam_s += res[i + w_sizeb, j] - res[i + w_sizeb, j - w_sizeb];
                    znam_s += res[i + w_sizeb, j + w_sizeb] - res[i + w_sizeb, j];
                    //************************************************************

                    //************************************************************
                    znam_s += res[i + w_sizeb, j - w_sizeb] - res[i, j - w_sizeb];
                    znam_s += res[i, j - w_sizeb] - res[i - w_sizeb, j - w_sizeb];

                    znam_s += res[i + w_sizeb, j] - res[i, j];
                    znam_s += res[i, j] - res[i - w_sizeb, j];

                    znam_s += res[i + w_sizeb, j + w_sizeb] - res[i, j + w_sizeb];
                    znam_s += res[i, j + w_sizeb] - res[i - w_sizeb, j + w_sizeb];
                    //************************************************************

                    //************************************************************
                    znam_s += res[i + w_sizeb, j + w_sizeb] - res[i, j];
                    znam_s += res[i, j] - res[i - w_sizeb, j - w_sizeb];

                    znam_s += res[i + w_sizeb, j] - res[i, j - w_sizeb];

                    znam_s += res[i, j + w_sizeb] - res[i - w_sizeb, j];
                    //************************************************************

                    znam_s = abs(znam_s);

                    resh1[k, l] /= znam_s;
                    resh2[k, l] /= znam_s;
                    resh3[k, l] /= znam_s;
                }
            }

            double min1, min2, min3, max1, max2, max3;

            min1 = Double.MaxValue;
            max1 = Double.MinValue;

            min2 = min1;
            max2 = max1;

            min3 = min1;
            max3 = max1;

            for (int i = 0; i < _zona_size; i++) //В цикле по размеру новых массивов
            {
                for (int j = 0; j < _zona_size; j++)
                {

                    if (min1 > resh1[i, j] && (abs(resh1[i, j]) < 1.2)) min1 = resh1[i, j];
                    if (min2 > resh2[i, j] && (abs(resh2[i, j]) < 1.2)) min2 = resh2[i, j];
                    if (min3 > resh3[i, j] && (abs(resh3[i, j]) < 1.2)) min3 = resh3[i, j];

                    if (max1 < resh1[i, j] && (abs(resh1[i, j]) < 1.2)) max1 = resh1[i, j];
                    if (max2 < resh2[i, j] && (abs(resh2[i, j]) < 1.2)) max2 = resh2[i, j];
                    if (max3 < resh3[i, j] && (abs(resh3[i, j]) < 1.2)) max3 = resh3[i, j];
                }
            }

            double oldRange1 = 255.0 / (max1 - min1);
            double oldRange2 = 255.0 / (max2 - min2);
            double oldRange3 = 255.0 / (max3 - min3);

            byte[] res_mas1 = new byte[_zona_size * _zona_size * 3];  //Новый масиив для копирования пикселей в изображение в формате Bgr24 (24 бита на пиксел)
            byte[] res_mas2 = new byte[_zona_size * _zona_size * 3]; //Новый масиив для копирования пикселей в изображение в формате Bgr24 (24 бита на пиксел)
            byte[] res_mas3 = new byte[_zona_size * _zona_size * 3]; //Новый масиив для копирования пикселей в изображение в формате Bgr24 (24 бита на пиксел)

            int p = 0;
            byte buf1;
            byte buf2;
            byte buf3;

            for (int i = 0; i < _zona_size; i++) //В цикле по размерам зоны анализа заполняем новые массивы
            {
                for (int j = 0; j < _zona_size; j++)
                {
                    buf1 = (byte)((resh1[i, j] - min1) * oldRange1);
                    buf2 = (byte)((resh2[i, j] - min2) * oldRange2);
                    buf3 = (byte)((resh3[i, j] - min3) * oldRange3);

                    if (resh1[i, j] > 1.2) buf1 = 255;
                    if (resh1[i, j] < -1.2) buf1 = 0;

                    if (resh2[i, j] > 1.2) buf2 = 255;
                    if (resh2[i, j] < -1.2) buf2 = 0;

                    if (resh3[i, j] > 1.2) buf3 = 255;
                    if (resh3[i, j] < -1.2) buf3 = 0;

                    res_mas1[p] = buf1;
                    res_mas1[p + 1] = buf1;
                    res_mas1[p + 2] = buf1;

                    res_mas2[p] = buf2;
                    res_mas2[p + 1] = buf2;
                    res_mas2[p + 2] = buf2;

                    res_mas3[p] = buf3;
                    res_mas3[p + 1] = buf3;
                    res_mas3[p + 2] = buf3;

                    p += 3;
                }
            }

            over_draw(res_mas1, res_mas2, res_mas3);
        }

        public void base_func_1(Thickness zone_margin)
        {
            //Получение массива байтов зоны анализа с учетом выхода окон за рамки
            short[,] mas = get_zone_data(zone_margin);

            int dzone = _zona_size + w_sizeb; //Размер зоны + размер окна пополам = количество функционалов для данной зоны
            double[,] res = new double[dzone, dzone]; //Массив основных функционалов

            double sum = 0; //Сумма по окну

            for (int i = 0; i < dzone; i++) //Заполнение массива основных функционалов     
            {
                for (int j = 0; j < dzone; j++)
                {
                    //Расчет функционала над(под) окном
                    for (int l = 0; l < _w_size; l++) //Для каждого элемента под окном     
                    {
                        for (int y = 0; y < _w_size; y++)
                        {
                            //Считаем произведение текущего элемента под окном на соответствующий вес и складываем в переменную для суммы
                            //Массив kxy хранит веса, данный массив генерируется функцией gen_kxy()
                            sum += mas[i + l, j + y] * kxy[l, y];
                        }
                    }

                    res[i, j] = sum; //Здесь можно округлить, но я не стал этого делать
                    sum = 0;
                }
            }

            //Для удобства создадим три новых двумерных массива размерностью зоны анализа
            double[,] resh1 = new double[_zona_size, _zona_size]; //Массив для горизонтальной пары окон
            double[,] resh2 = new double[_zona_size, _zona_size]; //Массив для вертикальной пары окон
            double[,] resh3 = new double[_zona_size, _zona_size]; //Массив для диагональной пары окон

            //Инициализируем переменные для дальнейшего вычисления диапазона получившихся массивов
            //Это необходимо для приведения значений в диапазон от 0 до 255
            double min1, min2, min3, max1, max2, max3;

            min1 = Double.MaxValue;
            max1 = Double.MinValue;

            min2 = min1;
            max2 = max1;

            min3 = min1;
            max3 = max1;

            for (int i = 0; i < _zona_size; i++) //В цикле по размеру новых массивов
            {
                for (int j = 0; j < _zona_size; j++)
                {
                    resh1[i, j] = res[i, j + w_sizeb] - res[i, j]; //Считаем разность горизонтальной пары окон
                    resh2[i, j] = res[i + w_sizeb, j] - res[i, j]; //Считаем разность вертикальной пары окон
                    resh3[i, j] = res[i + w_sizeb, j + w_sizeb] - res[i, j]; //Считаем разность диагональной пары окон

                    //Поиск минимальных элементов
                    if (min1 > resh1[i, j]) min1 = resh1[i, j];
                    if (min2 > resh2[i, j]) min2 = resh2[i, j];
                    if (min3 > resh3[i, j]) min3 = resh3[i, j];
                    //Поиск максимальных элементов
                    if (max1 < resh1[i, j]) max1 = resh1[i, j];
                    if (max2 < resh2[i, j]) max2 = resh2[i, j];
                    if (max3 < resh3[i, j]) max3 = resh3[i, j];
                }
            }

            //Рассчитаем коэффиценты приведения к нужному диапазону
            double hk1 = (255 / (max1 - min1));
            double hk2 = (255 / (max2 - min2));
            double hk3 = (255 / (max3 - min3));

            //Новые растровые массивы изображений в формате Bgr24 (24 разряда на пиксел)
            byte[] res_mas1 = new byte[_zona_size * _zona_size * 3];
            byte[] res_mas2 = new byte[_zona_size * _zona_size * 3];
            byte[] res_mas3 = new byte[_zona_size * _zona_size * 3];

            int p = 0;
            byte buf1;
            byte buf2;
            byte buf3;

            for (int i = 0; i < _zona_size; i++) //В цикле по размерам зоны анализа заполняем новые массивы
            {
                for (int j = 0; j < _zona_size; j++)
                {
                    //При этом тут же приводим каждое исходное значение к нужному диапазону
                    buf1 = (byte)((resh1[i, j] - min1) * hk1);
                    buf2 = (byte)((resh2[i, j] - min2) * hk2);
                    buf3 = (byte)((resh3[i, j] - min3) * hk3);

                    res_mas1[p] = buf1;
                    res_mas1[p + 1] = buf1;
                    res_mas1[p + 2] = buf1;

                    res_mas2[p] = buf2;
                    res_mas2[p + 1] = buf2;
                    res_mas2[p + 2] = buf2;

                    res_mas3[p] = buf3;
                    res_mas3[p + 1] = buf3;
                    res_mas3[p + 2] = buf3;

                    p += 3;
                }
            }

            over_draw(res_mas1, res_mas2, res_mas3);
        }

        short[,] get_zone_data_ex(Thickness zone_margin)
        {
            int lbzone = _zona_size + _w_size + _w_size - 1;

            //Расчет дополнительных смещений
            int x_r = (int)(zone_margin.Left - _w_size);
            int y_r = (int)(zone_margin.Top - _w_size);

            int xd_r = (int)(zone_margin.Left + lbzone - w_sizeb);
            int yd_r = (int)(zone_margin.Top + lbzone - w_sizeb);

            if (x_r >= 0 && y_r >= 0 && xd_r <= bmp_main.PixelWidth && yd_r <= bmp_main.PixelHeight)
            {
                //rs - результирующий массив
                short[,] rs = new short[lbzone, lbzone];
                
                int now_str;
                for (int i = 0; i < lbzone; i++)
                {
                    now_str = bmp_main.PixelWidth * (i + (int)zone_margin.Top) + (int)zone_margin.Left - w_sizeb;
                    for (int j = 0; j < lbzone; j++)
                        rs[i, j] = r_date[now_str + j];
                }
                return rs;
            }
            else //Иначе
            {
                //rs - результирующий массив необходимого размера
                short[,] rs = new short[lbzone, lbzone];
                int cl = lbzone - _zona_size - _w_size;
                //Зполнение результирующего массива расровыми данными фрагмента с учетом смещения
                int now_str;
                for (int i = _w_size, y = 0; i < lbzone - cl; i++, y++)
                {
                    now_str = (y + (int)zone_margin.Top) * bmp_main.PixelWidth + (int)zone_margin.Left;
                    for (int j = _w_size, x = 0; j < lbzone - cl; j++, x++)
                        rs[i, j] = r_date[now_str + x];
                }

                //Заполнение остальных зон результирующего массива
                for (int i = _w_size; i < lbzone - cl; i++)
                    for (int j = 0; j < _w_size; j++)
                        rs[i, j] = rs[i, j + _w_size];

                for (int i = _w_size; i < lbzone - cl; i++)
                    for (int j = lbzone - cl; j < lbzone; j++)
                        rs[i, j] = rs[i, j - cl];

                for (int i = 0; i < _w_size; i++)
                    for (int j = 0; j < lbzone; j++)
                        rs[i, j] = rs[i + _w_size, j];

                for (int i = lbzone - cl; i < lbzone; i++)
                    for (int j = 0; j < lbzone; j++)
                        rs[i, j] = rs[i - cl, j];

                return rs;
            }
        }

        short[,] get_zone_data(Thickness zone_margin)
        {
            int x_r = (int)(zone_margin.Left - w_sizeb);
            int y_r = (int)(zone_margin.Top - w_sizeb);

            int xd_r = (int)(zone_margin.Left + bzone - (w_sizeb));
            int yd_r = (int)(zone_margin.Top + bzone - (w_sizeb));

            if (x_r >= 0 && y_r >= 0 && xd_r <= bmp_main.PixelWidth && yd_r <= bmp_main.PixelHeight)
            {
                short[,] rs = new short[bzone, bzone];
                int now_str;
                for (int i = (int)zone_margin.Top - w_sizeb, y = 0; i < (int)zone_margin.Top - w_sizeb + bzone; i++, y++)
                {
                    now_str = i * bmp_main.PixelWidth + (int)zone_margin.Left - w_sizeb;
                    for (int j = 0; j < bzone; j++)
                        rs[y, j] = r_date[now_str + j];
                }
                return rs;
            }
            else
            {
                short[,] rs = new short[bzone, bzone];
                int dl = w_sizeb;
                int cl = bzone - _zona_size - dl;
                int now_str;
                for (int i = dl, x = 0; i < bzone - cl; i++, x++)
                {
                    now_str = (x + (int)zone_margin.Top) * bmp_main.PixelWidth + (int)zone_margin.Left;
                    for (int j = dl, y = 0; j < bzone - cl; j++, y++)
                        rs[i, j] = r_date[now_str + y];
                }

                for (int i = dl; i < bzone - cl; i++)
                    for (int j = 0; j < dl; j++)
                        rs[i, j] = rs[i, j + dl];

                for (int i = dl; i < bzone - cl; i++)
                    for (int j = bzone - cl; j < bzone; j++)
                        rs[i, j] = rs[i, j - cl];

                for (int i = 0; i < dl; i++)
                    for (int j = 0; j < bzone; j++)
                        rs[i, j] = rs[i + dl, j];

                for (int i = bzone - cl; i < bzone; i++)
                    for (int j = 0; j < bzone; j++)
                        rs[i, j] = rs[i - cl, j];

                return rs;
            }
        }

        public static double abs(double b) => b > 0 ? b : b * -1;

        public static int abs(int b) => b > 0 ? b : b * -1;

        void over_draw(byte[] rm_h, byte[] rm_v, byte[] rm_d)
        {
            bmp_result_h = new WriteableBitmap(_zona_size, _zona_size, 96, 96, PixelFormats.Bgr24, null);
            Int32Rect nrect = new Int32Rect(0, 0, _zona_size, _zona_size);
            bmp_result_h.WritePixels(nrect, rm_h, _zona_size * 3, 0);

            bmp_result_v = new WriteableBitmap(_zona_size, _zona_size, 96, 96, PixelFormats.Bgr24, null);
            bmp_result_v.WritePixels(nrect, rm_v, zona_size * 3, 0);

            bmp_result_d = new WriteableBitmap(_zona_size, _zona_size, 96, 96, PixelFormats.Bgr24, null);
            bmp_result_d.WritePixels(nrect, rm_d, _zona_size * 3, 0);
        }
    }
}
