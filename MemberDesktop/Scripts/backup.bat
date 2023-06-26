echo off
cd c:\Program Files\MySQL\MySQL Server 8.0\bin
cd
mysqldump -u root -picne icne > c:\ICNE\DBBackup\%1.sql
