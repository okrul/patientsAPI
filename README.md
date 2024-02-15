#Docker Images:

##MySQL-сервер и тестовая БД:
docker pull aokrul/mysql
docker run -dt -p 3308:3306 aokrul/patientsapi:latest
Порт 3308 используется в API сервисе

##REST API сервис:
docker pull aokrul/patientsapi
docker run -dt -p 32804:80 aokrul/patientsapi:latest
Порт 32804 - исползуется в генераторе

##Генератор пациентов:
docker pull aokrul/generator 
docker run -dt -P aokrul/generator:latest