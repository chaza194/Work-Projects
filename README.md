
# Descriptions Of Projects 
 
## RatesMultiThread -  
 
  At my current place of work I need to input current currency exchange rates for testing purposes. So to stop me from constantly opening   chrome all the time and searching for an exchange rate, I created a window that will display (on a hot-key press) all the currencies and   stock rates I need at a given time. Each currency exchange conversion runs on its own thread calling a free API to get the current       exchange rates and will refresh at individually set times. 
 

## DataMapper -   
 
*Please Note the code / directories have not been cleaned yet. This project is still in rough stages* 
 
This tool is used to export data from a SQL data into JSON based on user mappings. For Example : 
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
 
## DBDirectoryMover -   
This is a quick tool that I wrote. It will move files from a given directory based on SQL provided. I dont see this being useful to anyone else but at my current place of work. 

## JsonInjector *Most Recent*-   
This project was to understand "SimpleInjector". It will read from multiple files from multiple threads and will processes the data from a Queue.	