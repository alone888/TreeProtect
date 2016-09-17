
#include "timer.h"


/*******************************************************************************/	  
//��ͨ�ö�ʱ��3 ����Ϊ������ģʽ

/*******************************************************************************/
void TIM3_Mode_Config(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;
	TIM_TimeBaseInitTypeDef  TIM_TimeBaseStructure;
	TIM_ICInitTypeDef TIM_ICInitStructure;
	//TIM_OCInitTypeDef  TIM_OCInitStructure;
	
	/*----------------------------------------------------------------*/
	
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOA, ENABLE);
	
	
	GPIO_StructInit(&GPIO_InitStructure);
	/* Configure PA.06,07 as encoder input */
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_6 | GPIO_Pin_7;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
	GPIO_Init(GPIOA, &GPIO_InitStructure);
	
	/*----------------------------------------------------------------*/        
	
	
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM3, ENABLE); //ʹ�ܣԣɣͣ�
	TIM_DeInit(TIM3);
	TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
	
	TIM_TimeBaseStructure.TIM_Period =0xffff;       //
	TIM_TimeBaseStructure.TIM_Prescaler =0;            //����Ԥ��Ƶ��
	TIM_TimeBaseStructure.TIM_ClockDivision =TIM_CKD_DIV1 ;        //����ʱ�ӷ�Ƶϵ��������Ƶ
	TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up;  //���ϼ���ģʽ
	//TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_CenterAligned1; 
	/*��ʼ��TIM2��ʱ�� */
	TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
	
	/*-----------------------------------------------------------------*/
	//��������                        ����ģʽ
	TIM_EncoderInterfaceConfig(TIM3, TIM_EncoderMode_TI12, 
	TIM_ICPolarity_Rising, TIM_ICPolarity_Rising);  //TIM_ICPolarity_Rising�����ز���


	TIM_ICStructInit(&TIM_ICInitStructure);

	TIM_ICInitStructure.TIM_Channel = TIM_Channel_1;//����ͨ��1���˲���
	TIM_ICInitStructure.TIM_ICFilter = 6;         	//�Ƚ��˲���
	TIM_ICInit(TIM3, &TIM_ICInitStructure);

	TIM_ICInitStructure.TIM_Channel = TIM_Channel_2;//����ͨ��2���˲���
	TIM_ICInitStructure.TIM_ICFilter = 6;         	//�Ƚ��˲���
	TIM_ICInit(TIM3, &TIM_ICInitStructure);
	
	TIM_ARRPreloadConfig(TIM3, ENABLE);
	// Clear all pending interrupts
	TIM_ClearFlag(TIM3, TIM_FLAG_Update);
	TIM_ITConfig(TIM3, TIM_IT_Update, ENABLE);   //ʹ���ж�
	//Reset counter
	TIM3->CNT =0;	
	TIM_Cmd(TIM3, ENABLE);   //ʹ�ܶ�ʱ��3
}
void TIM_Init(void)
{
	TIM3_Mode_Config();
}
/*******************************************************************************/	  
//���߼����ƶ�ʱ��1 ����Ϊ������ģʽ

/*******************************************************************************/
void TIM1_Mode_Config(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;
	TIM_TimeBaseInitTypeDef  TIM_TimeBaseStructure;
	TIM_ICInitTypeDef TIM_ICInitStructure;
	//TIM_OCInitTypeDef  TIM_OCInitStructure;
	
	/*-------------��һ��-------------------------------------------------------------------------------------------------------------------*/
	//�������� RCC ����ʱ�� �������ġ�  �������ö�Ӧ���ŵ�GPIO�� timer1 ��Ӧ��CH1 �� CH2 �ֱ�Ϊ PE9 �� PE11 ��������������GPIOE
	// һ��timer ��Ӧ�����ĸ�����ͨ�� CH1 ~ CH4
	/*---------------------------------------------------------------------------------------------------------------------------------------*/
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOE, ENABLE);
	
	/*-------------�ڶ���-------------------------------------------------------------------------------------------------------------------*/
	//timer1 û����ӳ���ʱ�� CH1 �� CH2 �ֱ��Ӧ������PA8 �� PA9  ���κ� PA9 �� PA10��������uart1�� ����
	//������Ҫ��ȫ��ӳ�䣬��CH1 �� CH2 �ֱ��Ӧ��PE9 �� PE11��  �����������ǰ��LPC2214��һ���� ���õ�������Ҫȥ�������������ʲô���ã�stm��оƬ����Ҫ
	// timer3 û��������� ���Բ���Ҫ��ӳ��
	/*---------------------------------------------------------------------------------------------------------------------------------------*/
	GPIO_PinRemapConfig(GPIO_FullRemap_TIM1,ENABLE);

	
	GPIO_StructInit(&GPIO_InitStructure);
	//��ӳ��ǰ������
//	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_8 | GPIO_Pin_9;
//	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
//	GPIO_Init(GPIOA, &GPIO_InitStructure);

	//��ȫ��ӳ��������
	/* Configure PE.09 11 as encoder input */
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9 | GPIO_Pin_11;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
	GPIO_Init(GPIOE, &GPIO_InitStructure);

	
	/*---------------������---------------------------------------------*/        
	//����timer1�� ��������
	//��Ϊtimer1 �õ�APB2��timer3�õ�APB1��
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_TIM1,ENABLE);
	
	
	 //ʹ��timer1
	TIM_DeInit(TIM1);
	TIM_TimeBaseInit(TIM1, &TIM_TimeBaseStructure);
	
	TIM_TimeBaseStructure.TIM_Period =0xffff;       //
	TIM_TimeBaseStructure.TIM_Prescaler =0;            				//����Ԥ��Ƶ��
	TIM_TimeBaseStructure.TIM_ClockDivision =TIM_CKD_DIV1 ;        	//����ʱ�ӷ�Ƶϵ��������Ƶ
	TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up;  	//���ϼ���ģʽ
	//TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_CenterAligned1; 
	/*��ʼ��TIM1��ʱ�� */
	TIM_TimeBaseInit(TIM1, &TIM_TimeBaseStructure);
	
	/*----------------���Ĳ�-------------------------------------------------*/
	//��timer1����Ϊ����ģʽ              TIM_EncoderMode_TI12����˼��AB�඼�ã�Ҳ����ֻ��һ��
	//TIM_EncoderInterfaceConfig(TIM1, TIM_EncoderMode_TI12, 
	//TIM_ICPolarity_Rising, TIM_ICPolarity_Rising);  //TIM_ICPolarity_Rising�����ز���    
	
	//---�����ϸ�ֵ� Ӧ�������½��ض�Ҫ���� ������
	TIM_EncoderInterfaceConfig(TIM1, TIM_EncoderMode_TI12, 
	TIM_ICPolarity_BothEdge, TIM_ICPolarity_BothEdge);  //TIM_ICPolarity_BothEdge�����ػ����½��ز���  

	//  ����AB��� �����˲���  
	//  ��������˲�������Ҫ���˳���Ƶ���ţ������ϴεĵ�Ÿ��� �����оƬ��������У�����!!
	TIM_ICStructInit(&TIM_ICInitStructure);

	TIM_ICInitStructure.TIM_Channel = TIM_Channel_1;//����ͨ��1���˲���
	TIM_ICInitStructure.TIM_ICFilter = 6;         	//�Ƚ��˲���   
	TIM_ICInit(TIM1, &TIM_ICInitStructure);

	TIM_ICInitStructure.TIM_Channel = TIM_Channel_2;//����ͨ��2���˲���
	TIM_ICInitStructure.TIM_ICFilter = 6;         	//�Ƚ��˲���
	TIM_ICInit(TIM1, &TIM_ICInitStructure);
	

	/*----------------���岽  ��β����-------------------------------------------------*/
	TIM_ARRPreloadConfig(TIM1, ENABLE);//�Զ���װ������
	// Clear all pending interrupts
	TIM_ClearFlag(TIM1, TIM_FLAG_Update);
	TIM_ITConfig(TIM1, TIM_IT_Update, ENABLE);   //ʹ���ж�
	//Reset counter
	TIM1->CNT =0;			//ֱ�Ӷ����counter ���Ǳ�������ֵ ֻ��16λ�� ���������32λ�ģ���Ϊ����Ա����϶����뵽�취�ġ�	
	TIM_Cmd(TIM1, ENABLE);   //ʹ�ܶ�ʱ��1	
}
