   M S B A M L   `   `   `  ���� A  =FM2WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:FM2WPFFM2WPF      FM2WPF.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     &localclr-namespace:FM2WPF       � Title$  FM2WPF��$��869.307��$��1126.505�� � WindowState$ 	Maximized=�.��� -    � ShowGridLines .   � ColumnDefinitions �� $	��180 ��� $��* �.+�� $��1�� �� �� $��3 ���  � RowDefinitions �� $��* ��� $��3 ��� $
��1.4* �.+��� $��Stretch=�$��Stretch=�$��5 0 5 5q�$��0��$��0��$��2��
�������$��3q�._��� -    ��Name$ 
scroll_img��$
��Auto=�$
��Auto=�.��� -    �Name$ img_grid��
 J� Uid$ g1��$��5 5 5 5q�
�������.+��� -    ��Name$ img_bw��$
��Left=�$	��Top=�$
��Fill=��� $��Stretch=�$��Stretch=�$��5 0 5 5q�$��0��$��3��$��2��
�������$��3q�._��� -   $ scroll_img_2��$
��Auto=�$
��Auto=�.��� -   $ 
img_grid_2��$ g2��$��5 5 5 5q�
�������.+��� -   $ img_bw_2��$
��Left=�$	��Top=�$
��Fill=�� $��2��$��0�� �� ��  �� �� .+��� -   $
 sv_h��$��Disabled=�$��Disabled=�$��0��$��0��$��Stretch=�$��Stretch=�
�������$��5q�.���� -	   $ img_h��$��Uniform=��� -
   $
 sv_v��$��Disabled=�$��Disabled=�$��0��$��1��$��Stretch=�$��Stretch=�
�������$��5q�.���� -   $ img_v��$��Uniform=��� -   $
 sv_d��$��Disabled=�$��Disabled=�$��1��$��0��$��Stretch=�$��Stretch=�
�������$��5q�.���� -   $ img_d��$��Uniform=��� $��2wh.png��$��55��$��55��$
��Left=�$	��Top=�$��5q�$��1��$��1���� $��2wv.png��$��55��$��55��$��Right=�$	��Top=�$��5q�$��1��$��1���� $��2wd.png��$��55��$��55��$
��Left=�$��Bottom=�$��5q�$��1��$��1���� $��1��$��0��$��3��$��Stretch=�$��Center=�$��3��	 �� ShowsPreview	 .  �� $��0��$��1��$��3��$��Center=�$��Stretch=�$��3��	 .  � $��2��$��2�� �� ��  �� �� .+��� -   $ sv_h2��$��Disabled=�$��Disabled=�$��0��$��0��$��Stretch=�$��Stretch=�
�������$��5q�.���� -   $ img_h_2��$��Uniform=��� -   $ sv_v2��$��Disabled=�$��Disabled=�$��0��$��1��$��Stretch=�$��Stretch=�
�������$��5q�.���� -   $ img_v_2��$��Uniform=��� -   $ sv_d2��$��Disabled=�$��Disabled=�$��1��$��0��$��Stretch=�$��Stretch=�
�������$��5q�.���� -   $ img_d_2��$��Uniform=��� $��2wh.png��$��55��$��55��$
��Left=�$	��Top=�$��5q�$��1��$��1���� $��2wv.png��$��55��$��55��$��Right=�$	��Top=�$��5q�$��1��$��1���� $��2wd.png��$��55��$��55��$
��Left=�$��Bottom=�$��5q�$��1��$��1���� $��Stretch=�$��Stretch=�$��2 0 0 5q�$��0��$��3��$��0��
�������$��3q�._��� $
��Auto=�.��� -   $ set_grid�� �� $��80 ��� $	��135 ��� $	��109 ��� $	��140 ��� $	��110 ��� $	��100 ��� $	��100 ��� $	��100 �.+��� -   
 ��Name$
 bt_open��$ 1��$#��Загрузить файл 1��$
��Left=�$	��Top=�$	��100��$��25��$��0��$��	15 10 0 0q��� -   $
 bt_open2��$ 2��$#��Загрузить файл 2��$
��Left=�$��Bottom=�$	��100��$��25��$��0��$��	15 0 0 10q��� -   $
 bt_test3��$	 bs1��$��Рассчитать 1��$
��Left=�$	��Top=�$��85��$��25��$��4��$��	15,10,0,0q��� -   $
 
bt_test3_2��$	 bs2��$��Рассчитать 2��$
��Left=�$	��Top=�$��85��$��25��$��4��$��	15,45,0,0q��� -    ��Name$ cb_map��$y�	Collapsed=�$��4��$
��Left=�$��Bottom=�$��	15 0 0 10q�$��Масштаб�� d� 	IsChecked$
 TrueY��� $.��(Размер первой области��$��1��$��10, 0, 0, 0q��� -    ��Name$ slider_zone��$ s1��$��Stretch=�$��1��$	��Top=�$��10, 25, 6, 10q� � Minimum$ 50Q� � Maximum$	 500Q� � Value$	 150Q� �� TickPlacement$ BottomRight=� �� AutoToolTipPlacement$ TopLeft=��� $.��(Размер второй области��$��1��$��10, 0, 0, 53q�$��Bottom=��� -   $ slider_zone_2��$ s2��$��Stretch=�$��1��$��Bottom=�$��10, 25, 6, 30q�$ 50Q�$	 500Q�$	 150Q�$ BottomRight=�$ TopLeft=��� -   $ cb_szone��$
��Left=�$��1��$��Bottom=�$��15 0 0 7q�$'��!Синхронизация зон��{�.  �� $,��&Размер окна анализа 1��$��2��$��10, 0, 0, 0q�$	��Top=��� -   $ slider_window��$	 sw1��$��Stretch=�$��2�� �� IsSnapToTickEnabled . $	��Top=�$��10, 25, 6, 10q�$ 8Q�$ 32Q�$ 8Q�$ BottomRight=�$ TopLeft=� �� TickFrequency$ 2Q� � LargeChange$ 2Q��� $,��&Размер окна анализа 2��$��2��$��10, 0, 0, 33q�$��Bottom=��� -   $ slider_window_2��$	 sw2��$��Stretch=�$��2�� . $��Bottom=�$��10, 25, 6, 10q�$ 8Q�$ 32Q�$ 8Q�$ BottomRight=�$ TopLeft=�$ 2Q�$ 2Q��� -    ��Name$ gb_mbv_offset��$��5��$&�� Сдвиг для .mbv рис. 1��$��2q�$��90��$	��Top=�{�.  .���� .+�	� -     	�Name$ rb_offset_0��$��сдвиг 0��$��5��$��3q�$
 TrueY�	� -!   $ rb_offset_1��$��сдвиг 1��$��5��$��3q�	� -"   $ rb_offset_2��$��сдвиг 2��$��5��$��3q��� -#   $ gb_mbv_offset_s��$��6��$&�� Сдвиг для .mbv рис. 2��$��2q�$��90��$	��Top=�{�.  .���� .+�	� -$   $ rb_offset_0_s��$��сдвиг 0��$��5��$��3q�$
 TrueY�	� -%   $ rb_offset_1_s��$��сдвиг 1��$��5��$��3q�	� -&   $ rb_offset_2_s��$��сдвиг 2��$��5��$��3q��� -'   $
 	bt_charts��$#��Полная вариация��$��7��$	��120��$��25��$��15,5,0,0q�$
��Left=�$	��Top=�{�.  � $��3�� �� $��35 ��� ��  �� $��* ��� $��4* �.+��� $)��#Базовый функционал��$��0��$��0��$��2��$��10, 0, 0, 0q�$��Center=�	� -(   $ rb_fb1��$��1��$��0��$��15, 0, 0, 0q�$��Center=�$
 TrueY�	� -)   $ rb_fb2��$��2��$��0��$��15, 0, 0, 0q�$��Center=��� $��1��$��1��$��
2w_bf1.png��$
��None=��� $��2��$��1��$��
2w_bf2.png��