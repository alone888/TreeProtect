#include "ds18b20.h"
#include "delay.h"	


/*
 * ��������DS18B20_GPIO_Config
 * ����  ������DS18B20�õ���I/O��
 * ����  ����
 * ���  ����
 */
 void DS18B20_GPIO_Config(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;	//GPIO
	RCC_APB2PeriphClockCmd(	RCC_APB2Periph_GPIOG, ENABLE );	 //ʹ��PORTA��ʱ��  
 
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;  //SPI CS
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;  //�����������
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_Init(GPIOG, &GPIO_InitStructure);
	/* Deselect the PA0 Select high */
	GPIO_SetBits(GPIOG,GPIO_Pin_0);
}
/*
 * ��������DS18B20_Mode_IPU
 * ����  ��ʹDS18B20-DATA���ű�Ϊ����ģʽ
 * ����  ����
 * ���  ����
 */
void DS18B20_Mode_IPU(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;	//GPIO
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;  //SPI CS
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU;  //�����������
	GPIO_Init(GPIOG, &GPIO_InitStructure);

}
/*
 * ��������DS18B20_Mode_Out
 * ����  ��ʹDS18B20-DATA���ű�Ϊ���ģʽ
 * ����  ����
 * ���  ����
 */
void DS18B20_Mode_Out(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;	//GPIO
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;  //SPI CS
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;  //�����������
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_Init(GPIOG, &GPIO_InitStructure);

}



/*
 *�������ӻ����͸�λ����
 */
void DS18B20_Rst(void)	   
{        
    /* IO����Ϊ�������*/	
	  DS18B20_Mode_Out(); 
	  /*��������480us�ĵ͵�ƽ��λ�ź� */
    DS18B20_DQ_OUT_Low; 
		delay_us(480);  
    /* �ڲ�����λ�źź��轫�������� */	
    DS18B20_DQ_OUT_High; 
	  delay_us(15);    
}

/*
 * ���ӻ����������ص�Ӧ������
 *�ӻ����յ������ĸ�λ�źź󣬻���15~60us���������һ��Ӧ������
 * 0���ɹ�
 * 1��ʧ��
 */
u8 DS18B20_Answer_Check(void) 	   
{   
	u8 delay=0;
	/* ��������Ϊ�������� */
	DS18B20_Mode_IPU(); 
	/* �ȴ�Ӧ�����壨һ��60~240us�ĵ͵�ƽ�ź� ���ĵ���
	 * ���100us�ڣ�û��Ӧ�����壬�˳�������ע�⣺�ӻ����յ������ĸ�λ�źź󣬻���15~60us���������һ����������
	 */
	while (DS18B20_DQ_IN&&delay<100)
	{
		delay++;
		delay_us(1);
	}	 
	/*����100us�����û��Ӧ�����壬�˳�����*/	
	if(delay>=100)
		return 1;
	else 
		delay=0;
	/*��Ӧ�����壬�Ҵ���ʱ�䲻����240us */
	while (!DS18B20_DQ_IN&&delay<240)
	{
		delay++;
		delay_us(1);
	}
	if(delay>=240)
		return 1;	    
	  return 0;
}

//��DS18B20��ȡһ��λ
//����ֵ��1/0
u8 DS18B20_Read_Bit(void) 			 // read one bit
{
  u8 data;
	DS18B20_Mode_Out();
	/* ��ʱ�����ʼ���������������� >1us <15us �ĵ͵�ƽ�ź� */
  DS18B20_DQ_OUT_Low; 
	delay_us(2);
	DS18B20_DQ_OUT_High; 
		delay_us(12);
	/* ���ó����룬�ͷ����ߣ����ⲿ�������轫�������� */
	DS18B20_Mode_IPU();

	if(DS18B20_DQ_IN)
		data=1;
  else 
		data=0;	 
  delay_us(50);           
  return data;
}
//��DS18B20��ȡһ���ֽ�
//����ֵ������������
u8 DS18B20_Read_Byte(void)    // read one byte
{        
    u8 i,j,dat;
    dat=0;
	for(i=0; i<8; i++) 
	{
		j = DS18B20_Read_Bit();		
		dat = (dat) | (j<<i);
	}					    
    return dat;
}
/*
 * дһ���ֽڵ�DS18B20
 */
void DS18B20_Write_Byte(u8 dat)     
 {             
    u8 j;
    u8 testb;
	DS18B20_Mode_Out();//SET PA0 OUTPUT;
    for (j=1;j<=8;j++) 
	{
        testb=dat&0x01;
        dat=dat>>1;
        if (testb) 
        {
            DS18B20_DQ_OUT_Low;// Write 1
            delay_us(10);                            
            DS18B20_DQ_OUT_High;
            delay_us(50);             
        }
        else 
        {
            DS18B20_DQ_OUT_Low;// Write 0
            delay_us(60);             
            DS18B20_DQ_OUT_High;   ///�ͷ�����
            delay_us(2);                          
        }
    }
}

//��ʼ��DS18B20��IO�� DQ ͬʱ���DS�Ĵ���
//����1:������
//����0:����    	 
u8 DS18B20_Init(void)
{
  DS18B20_GPIO_Config();
	DS18B20_Rst();
	return DS18B20_Answer_Check();
}  
//��ds18b20�õ��¶�ֵ
//���ȣ�0.1C
//����ֵ���¶�ֵ ��-550~1250�� 
short DS18B20_Get_Temp(void)
{
    
	u8 flag;
	u8 TL,TH;
	short Temperature;
	float Temperature1;
	DS18B20_Rst();	   
	DS18B20_Answer_Check();	 
	DS18B20_Write_Byte(0xcc);// skip rom
	DS18B20_Write_Byte(0x44);// convert                 // ds1820 start convert
	DS18B20_Rst();
	DS18B20_Answer_Check();	 
	DS18B20_Write_Byte(0xcc);// skip rom
	DS18B20_Write_Byte(0xbe);// convert	    
	TL=DS18B20_Read_Byte(); // LSB   
	TH=DS18B20_Read_Byte(); // MSB  
	if( TH&0xfc)
	{
	flag=1;
	Temperature=(TH<<8)|TL;
	Temperature1=(~ Temperature)+1;
	Temperature1*=0.0625;
	}
	else
	{
	flag=0;
	Temperature1=((TH<<8)|TL)*0.0625;
	}
	Temperature = Temperature1*100;
	return Temperature; 	
} 
 
