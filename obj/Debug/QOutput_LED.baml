   M S B A M L   `   `   `  ����N  JWPF_LogicSimulation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WPF_LogicSimulationWPF_LogicSimulation  5      9!clr-namespace:WPF_LogicSimulationWPF_LogicSimulation  5      %    WPF_LogicSimulation.QOutput_LED   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      1local!clr-namespace:WPF_LogicSimulation    5      /src!clr-namespace:WPF_LogicSimulation    5        � 	Resources  5
      &    WPF_LogicSimulation.CQBool2Brush   
bool2brush   True   False  bool2brush_simulate  bool2brush_endpoint*   $WPF_LogicSimulation.CQBoolVisibility  bool2visibility*   $WPF_LogicSimulation.CQBool2Thickness  bool2thickness��%�   &	        &	 )     &	 R     &	 {     &	 �       5   
   
 � ���
 �   �6<     5   
   
 � � �
 �  ��6D     5   
   
 ����
 �����6I     5   
   6     5   
   6   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   �� -   5         ��Name$ itemscontrol_in��5      $��Right=�6O   ��60   �� *Pin_in+��5      �� 5      .R�5      ��  ��Name$ 	rectangle��6    -   5        
���   �$��10��5   �   $��10��6�   $��1��6�   ��63   �� *IsTrue+ �� 	Converter# [ 5      5      ��5      �� 5      .R�5      A�  A� Columns$ 1��6"   5       5"      � 5#   
   $��1��6   .+�5$      �  � RowDefinitions 5%      �� 5&      �� 5'      .+�5)      �� 
�������$��1��6)   $��2��69   ��6J   �� *
IsSimulate+# [  ��6�   �� *
IsSimulate+# [ �� 5*      $��0,3,0,0q�66   $��Center=�6G   ��6   �� *
GateName+6G   A� 5+      $��1��6   $��5,0,5,0q�6y   ��6'   �� *Pin_in[0].IsTrue+# [ 6y   5,      5-      5.      5/      