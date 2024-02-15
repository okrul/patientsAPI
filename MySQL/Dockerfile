# Используем базовый образ MySQL
FROM mysql:latest

# Устанавливаем переменные окружения
ENV MYSQL_ROOT_PASSWORD=1qaz@WSX3edc
ENV MYSQL_DATABASE=example_db

# Копируем SQL-скрипт для заполнения базы данных в директорию /docker-entrypoint-initdb.d
COPY ./init.sql /docker-entrypoint-initdb.d/
