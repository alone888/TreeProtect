#include "calib.h"


////-5000mv �� 5000mv  ��� 500mv�ɼ�һ�����ʵ��λ��	  ���ܳ���6��
////-5000 -4500 -4000 -3500 -3000 -2500 -2000 -1500 -1000 -500 0 -----��11����
////5000 4500 4000 3500 3000 2500 2000 1500 1000 500-----��10����	����һ����21����
//s16 IndCalib[5][21];
////IndID  ��е�ID ��0~3
//s16 Volt2Distance(u8 IndID,s16 v)
//{
//	int i=0;
//	s16 low,up;
//	s16 Dis;
//	if (v>=5000)
//	{
//		Dis = IndCalib[IndID][20]+(v-5000)*(IndCalib[IndID][20] - IndCalib[IndID][19])/500; 
//		return Dis;	
//	}
//
//	if (v<-5000)
//	{
//		Dis = IndCalib[IndID][0]-(v-(-5000))*(IndCalib[IndID][0] - IndCalib[IndID][1])/500;
//		return Dis;	 
//	}
//	
//	for(i=0;i<20;i++)//��Ϊ��i+1 ����21��Խ��
//	{
//		low = -5000+500*i;
//		up = -5000+500*(i+1);
//		if(v>=low && v<up)
//		{
//			Dis = IndCalib[IndID][i]+(v-low)*(IndCalib[IndID][i+1] - IndCalib[IndID][i])/500;
//			return Dis;	 	
//		}
//	}
//	return 0;
//}

//-5000mv �� 5000mv  ��� 500mv�ɼ�һ�����ʵ��λ��	  ���ܳ���6��
//-5000 -4500 -4000 -3500 -3000 -2500 -2000 -1500 -1000 -500 0 -----��11����
//5000 4500 4000 3500 3000 2500 2000 1500 1000 500-----��10����	����һ����21����
s16 IndCalib[5][21];
//IndID  ��е�ID ��0~3
s16 Volt2Distance(u8 IndID,s16 v)
{
	int i=0;
	s16 low,up;
	s16 Dis;
	if (v>=IndCalib[IndID][20])
	{
		if(IndCalib[IndID][20] == IndCalib[IndID][19])
		{
			return 5000;
		}
		
		Dis = 5000+(v-IndCalib[IndID][20])*500/(IndCalib[IndID][20] - IndCalib[IndID][19]); 
		return Dis;	
	}

	if (v<IndCalib[IndID][0])
	{
		if(IndCalib[IndID][0] == IndCalib[IndID][1])
		{
			return -5000;
		}
		Dis = -5000-(v-IndCalib[IndID][0])*500/(IndCalib[IndID][0] - IndCalib[IndID][1]);
		return Dis;	 
	}
	
	for(i=0;i<20;i++)//��Ϊ��i+1 ����21��Խ��
	{
		low = -5000+500*i;
		up = -5000+500*(i+1);
		if(v>=IndCalib[IndID][i] && v<IndCalib[IndID][i+1])
		{
			if(IndCalib[IndID][i+1] == IndCalib[IndID][i])
			{
				return low;//��ĸΪ0	
			}
			Dis = low+(v-IndCalib[IndID][i])*500/(IndCalib[IndID][i+1] - IndCalib[IndID][i]);
			return Dis;	 	
		}
	}
	return 0;
}


void wr_IndCalib_to_flash(void)
{
	//Flash_Write(g_FlashAddr,(u8 *)IndCalib,105*2);	
}
void rd_IndCalib_from_flash(void)
{
	//Flash_Read(g_FlashAddr,(u8 *)IndCalib,105*2);	
}

//void wr_local_id_to_flash(void)
//{
//	Flash_Write(FLASH_MODBUD_LOCAL_ID,&(modbus.localID),1);
//}
//void wr_grat_bound_up_to_flash(void)
//{
//	Flash_Write(FLASH_GRAT_BOUND_UP,grat_info.grat[0].BOUND_UP.B.byte1,1);	
//	Flash_Write(FLASH_GRAT_BOUND_UP+2,grat_info.grat[0].BOUND_UP.B.byte2,1);
//	Flash_Write(FLASH_GRAT_BOUND_UP+4,grat_info.grat[0].BOUND_UP.B.byte3,1);
//	Flash_Write(FLASH_GRAT_BOUND_UP+6,grat_info.grat[0].BOUND_UP.B.byte4,1);
//}
//void wr_grat_bound_dn_to_flash(void)
//{
//	Flash_Write(FLASH_GRAT_BOUND_DN,grat_info.grat[0].BOUND_DN.B.byte1,1);	
//	Flash_Write(FLASH_GRAT_BOUND_DN+2,grat_info.grat[0].BOUND_DN.B.byte2,1);
//	Flash_Write(FLASH_GRAT_BOUND_DN+4,grat_info.grat[0].BOUND_DN.B.byte3,1);
//	Flash_Write(FLASH_GRAT_BOUND_DN+6,grat_info.grat[0].BOUND_DN.B.byte4,1);
//}
//void rd_local_id_from_flash(void)
//{
//	modbus.localID = *FLASH_MODBUD_LOCAL_ID;
//}
//void rd_grat_bound_up_from_flash(void)
//{
//	grat_info.grat[0].BOUND_UP.B.byte1 = *FLASH_GRAT_BOUND_UP;
//	grat_info.grat[0].BOUND_UP.B.byte2 = *FLASH_GRAT_BOUND_UP+2;
//	grat_info.grat[0].BOUND_UP.B.byte3 = *FLASH_GRAT_BOUND_UP+4;
//	grat_info.grat[0].BOUND_UP.B.byte4 = *FLASH_GRAT_BOUND_UP+6;
//}
