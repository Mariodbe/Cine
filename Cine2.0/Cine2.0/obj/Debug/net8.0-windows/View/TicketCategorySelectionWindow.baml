   M S B A M L   `   `   `  ����  CineApp0    *CineApp.View.TicketCategorySelectionWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$(  "MagicFilm - Selección de Entradas��5   	   $	��400��5   	   $	��530��6   ���#83110C � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��0,0,0,-6q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� -   5        $��Volver��5      $
��Left=�6"   $	��Top=�6=   $��10q�6U   $��80��5      $��40��6   $��0��6A   
���   ����#FFD700$
��Bold"�6v   $��0q�6�   �� 5"   
   $��Center=�6   $	��Top=�61   $��0,42,0,0q�6I   $��22(�5#      $
��Bold"�6"   $#��Selecciona el tipo de entrada��64   $��1��6Y   
�������6f   �� 5&   
   $	��Top=�6   $��Center=�6-   $
��0,10q�6J   $��2��5'      .+�5(      �� $��Entrada General (11€)��6   $��16(�65   
�������6C   �� 5)      -   5        $��GeneralTicketsComboBox��5)      $	��200��65   $��40��6A   $��Center=�6M   
���   ����#5A5A7A$��0q�5*   l   .@�5+      �� $��0��6   
 � Tag$ 0��6+   
����������#5A5A7A6F   �� 5,      $��1��6   $ 11��6+   
����������#5A5A7A6G   �� 5-      $��2��6   $ 22��6+   
����������#5A5A7A6G   �� 5.      $��3��6   $ 33��6+   
����������#5A5A7A6G   �� 5/      $��4��6   $ 44��6+   
����������#5A5A7A6G   �� 50      $��5��6   $ 55��6+   
����������#5A5A7A6G   51      52      �� 55   
   $	��Top=�6   $��Center=�6-   $
��0,10q�6J   $��3��56      .+�57      �� $ ��Menores de 13 años (8€)��6   $��16(�67   
�������6E   �� 58      -   5         ��Name$ Under13TicketsComboBox��58      $	��200��67   $��40��6C   $��Center=�6O   
���   ����#5A5A7A$��0q�59   l   .@�5:      �� $��0��6   $ 0��6+   
����������#5A5A7A6F   �� 5;      $��1��6   $ 8��6+   
����������#5A5A7A6F   �� 5<      $��2��6   $ 16��6+   
����������#5A5A7A6G   �� 5=      $��3��6   $ 24��6+   
����������#5A5A7A6G   �� 5>      $��4��6   $ 32��6+   
����������#5A5A7A6G   �� 5?      $��5��6   $ 40��6+   
����������#5A5A7A6G   5@      5A      �� 5D   
   $	��Top=�6   $��Center=�6-   $
��0,10q�6J   $��4��5E      .+�5F      �� $ ��Mayores de 65 años (5€)��6   $��16(�67   
�������6E   �� 5G      -   5        $��Over65TicketsComboBox��5G      $	��200��64   $��40��6@   $��Center=�6L   
���   ����#5A5A7A$��0q�5H   l   .@�5I      �� $��0��6   $ 0��6+   
����������#5A5A7A6F   �� 5J      $��1��6   $ 5��6+   
����������#5A5A7A6F   �� 5K      $��2��6   $ 10��6+   
����������#5A5A7A6G   �� 5L      $��3��6   $ 15��6+   
����������#5A5A7A6G   �� 5M      $��4��6   $ 20��6+   
����������#5A5A7A6G   �� 5N      $��5��6   $ 25��6+   
����������#5A5A7A6G   5O      5P      �� 5S   
   -   5        $��TotalPriceTextBlock��5S      $��Center=�6/   $	��Top=�6L   $
��0,10q�6d   $��16(�5T      $
��Bold"�6"   $��5��64   
�������6A   �� 5W   
   -   5        $ ��ValidationMessageTextBlock��5W      $��Center=�66   $	��Top=�6S   $
��0,10q�6k   $��14(�5X      
���  ��$y�	Collapsed=�63   $��6��6J   �� 5[   
   -   5        $��	Confirmar��5[      $��Center=�6%   $��Center=�6B   $	��150��5\      $��50��6   $��7��6H   
���   ����#FFD700$
��Bold"�6}   $��0q�6�   5]      5^      