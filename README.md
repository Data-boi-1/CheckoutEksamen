# Start applikationen og du scanner varer 1 af gangen.
- Du kan indtaste a-z(småt) for varer og stopper ved Q
- Der bliver udskrevet en kvittering i terminalen/konsollen.

## Beskrivelse:

I kassen i supermarkedet bliver varer scannet ind og priserne bliver beregnet. Varerne scannes på baggrund af en EAN-kode, men i denne opgave kalder vi dem bare ved et bogstav ‘A’ til ‘Z’ for at gøre det nemmere.

Prisen på varerne er stykpriser, men der dels nogle hvor de sælges i multipack (f.eks. varekode R svarer til 6 stk varekode F). Der er også kampagnepriser, hvor merkøb udløser rabat, f.eks. køb 3 betal for 2, eller køb 2stk for kr 130, 1 stk = 100 . Endelig er der også enkelte varer, som automatisk udløser en pant (Pant er f.eks. varekode P). Alle varer er inddelt i varegrupper 1-9, hvor f.eks. 1 er mejeri, 2 er kolonial etc

I kassen bliver varerne scannet i vilkårlig rækkefølge. Det kunne være f.eks. en vare B, så en vare A, og så en vare B mere, og hvis der var rabat på 2 B, så skal den selvfølgelig fremgå af prisen.

Modeller disse klasser, og lav en scanner, der kan tage imod varer fra konsollen og scanne varerne igennem (Lav et simpelt delay, så der går 500 ms mellem hver vare).

Lad scanneren sende et event til en prisberegner, som udregner en ny pris på baggrund af disse varer.

Implementer 2 prisberegnere, den billige og den dyre. Den billige vil bare udregne en total, som den viser hver gang. Den dyre vil vise listen af solgte varer, grupperet efter varegrupper og ordnet i samlet antal styk, så den samme varetype ikke indgår flere gange.

Krav til implementeringen:
Der skal bruges delegates til event
Der skal bruges LINQ til sortering/grupperinger af collections.
Der skal anvendes Properties og Object Initializers
