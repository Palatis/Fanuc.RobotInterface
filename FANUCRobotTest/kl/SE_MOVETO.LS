/PROG  SE_MOVETO
/ATTR
OWNER		= MNEDITOR;
COMMENT		= "";
PROG_SIZE	= 314;
CREATE		= DATE 22-05-15  TIME 03:43:10;
MODIFIED	= DATE 22-05-15  TIME 04:01:54;
FILE_NAME	= ;
VERSION		= 0;
LINE_COUNT	= 13;
MEMORY_SIZE	= 766;
PROTECT		= READ_WRITE;
TCD:  STACK_SIZE	= 0,
      TASK_PRIORITY	= 50,
      TIME_SLICE	= 0,
      BUSY_LAMP_OFF	= 0,
      ABORT_REQUEST	= 0,
      PAUSE_REQUEST	= 0;
DEFAULT_GROUP	= 1,*,*,*,*;
CONTROL_CODE	= 00000000 00000000;
/APPL
/MN
   1:  SELECT R[1]=1,JMP LBL[1] ;
   2:         =2,JMP LBL[2] ;
   3:         ELSE,JMP LBL[99] ;
   4:   ;
   5:  LBL[1] ;
   6:J PR[1] 100% FINE    ;
   7:  JMP LBL[99] ;
   8:   ;
   9:  LBL[2] ;
  10:L PR[1] 4000mm/sec FINE    ;
  11:  JMP LBL[99] ;
  12:   ;
  13:  LBL[99] ;
/POS
/END
