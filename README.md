
# Descriptions of projects 
 
## RatesMultiThread -  
 
### Description 
  At my current place of work I need to input current currency exchange rates for testing purposes. So to stop me from constantly opening   chrome all the time and searching for an exchange rate, I created a window that will display (on a hot-key press) all the currencies and   stock rates I need at a given time. Each currency exchange conversion runs on its own thread calling a free API to get the current       exchange rates and will refresh at individually set times. 
 
 
### Features 
- Real time exchange rates at a user defined refresh rate. 
- Real time Stock Opening rates at a user defined refresh rate. 
- All rates are calling from a separate thread. 
- Hot Key to hide and display window ("cntrl + num0") 
 
### Need To Do 
- Configuration for Hot Key as it is static. 
- Configuration for directory of ini file. 
 
### Image 
 
![alt text](https://preview.ibb.co/mtqSqJ/Rates.png) 
 
## DataMapper -   
 
*Please Note the code / directories have not been cleaned yet. Still in rough stages* 
 
This one is my most recent and currently working on. It is every rough and is not complete but the base functionality is mostly there. This tool is used to export data from a SQL data into JSON based on user mappings. For Example : 
``` 
--Base Query 
  --FieldA (returned form parent SQL)   
  --FieldB 
    --Base Query (parms based on parent node value) 
      --FieldA 
      --FieldB      
  --FieldC 
``` 
 
Serialization is all dynamic, everything is built from data returned from selected SQL tables and user defined mappings. 
 
### Features 
- Serialization of dynamically built objects based on user mappings.  
- User mappings build from user interface. 
- All changes to mappings are save to XML. 
 
### To Do 
- Clean Up Code. 
- Export JSON (only can be seen in debugging) 
- Remove view models to own .cs files  
- Make UI pretty. 
 
 
