   M S B A M L   `   `   `  ����O  KEevee connection map, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Eevee_connection_mapEevee_connection_map  5   	   %    Eevee_connection_map.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:Eevee_connection_map     5   	     � Title$  
MainWindow��5   	   $��731.619��6   $��1504.96��6-   #��� ��6=   #��� ��6�    � WindowStyle$
 None=�6�    � 
ResizeMode$ NoResize=�6�    J� Effect 5	      3  -System.Windows.Media.Effects.DropShadowEffect  5
      .��5      � $��	0,0,0,-31q�6   #��� ��6    � RowDefinitions 5      �� 5      $
��131* �6   �� 5      $
��188* �6   .+�5      �� -   5         ��Name$ connectListBox��5      $��10,34,0,194q�6$   $	��295��69   $��2��6E   $
��Left=�6V   $��Stretch=�6q   #��� ��6�   �� 5      -   5         ��Name$ map_img��5      $��310,34,10,37q�6   $��2��62   ��5      �� 5       �� ImageSource$' !world.topo.200412.3x5400x2700.jpg��6   5          Eevee_connection_map.popup  5      -   5          Name$ popmenu��5      $
��Left=�6!   $	��152��6<   $��
10,261,0,0q�6I   $��1��6]   $	��Top=�6j   $	��295��6�   #��� ��6�   $��1q�6�   }�. 6�   �� 5      -   5        $��X��5      $
��Left=�6   $��
1468,0,0,0q�62   $	��Top=�6F   $��37��6^   #��� ��6i   	 J� RenderTransformOrigin$	 1.211,0.825&�6�   $��29��6�   6�   �� 5      $$��EEVEE: Connection World Viewer��6
   $
��Left=�63   $��10,1,0,0q�6N   $	��Top=�6`   $��18(�6x   $��Terminator Two)�6�   5      5      