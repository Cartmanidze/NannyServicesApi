# 🏪 Nanny Services API

RESTful HTTP API для управления клиентами, продуктами и заказами, разработанный в стиле Domain Driven Design (DDD) с использованием современных технологий .NET.

## 🔧 Быстрый старт

### 1. Клонирование репозитория
```bash
git clone <repository-url>
cd NannyServicesApi
```

### 2. Восстановление пакетов
```bash
dotnet restore
```

### 3. Создание базы данных
```bash
# Переходим в Infrastructure проект
cd src/NannyServices.Infrastructure

# Применяем миграции
dotnet ef database update --startup-project ../NannyServices.Api
```

### 4. Запуск приложения
```bash
# Из корневой папки
dotnet run --project src/NannyServices.Api

# Или из папки API
cd src/NannyServices.Api
dotnet run
```

### 5. Открытие Swagger UI
Перейдите по адресу: **https://localhost:7180/swagger** или **http://localhost:5168/swagger**


## 📋 API Endpoints

### 👤 Customers (Клиенты)

| Метод | URL | Описание |
|-------|-----|----------|
| `GET` | `/api/customers` | Список клиентов с пагинацией |
| `GET` | `/api/customers/{id}` | Получить клиента по ID |
| `GET` | `/api/customers/{id}/with-orders` | Клиент со всеми заказами |
| `GET` | `/api/customers/search?searchTerm={term}` | Поиск по имени |
| `POST` | `/api/customers` | Создать нового клиента |
| `PUT` | `/api/customers/{id}` | Обновить клиента |
| `DELETE` | `/api/customers/{id}` | Удалить клиента |

#### 📊 Отчеты:
| Метод | URL | Описание |
|-------|-----|----------|
| `GET` | `/api/customers/{id}/reports?startDate={date}&endDate={date}` | Отчет за период |
| `GET` | `/api/customers/{id}/reports/week` | Отчет за текущую неделю |
| `GET` | `/api/customers/{id}/reports/month` | Отчет за текущий месяц |

### 🛍️ Products (Продукты)

| Метод | URL | Описание |
|-------|-----|----------|
| `GET` | `/api/products` | Список продуктов с пагинацией |
| `GET` | `/api/products/all` | Все продукты без пагинации |
| `GET` | `/api/products/{id}` | Получить продукт по ID |
| `GET` | `/api/products/search?searchTerm={term}` | Поиск по названию |
| `POST` | `/api/products` | Создать новый продукт |
| `PUT` | `/api/products/{id}` | Обновить продукт |
| `DELETE` | `/api/products/{id}` | Удалить продукт |

### 📦 Orders (Заказы)

| Метод | URL | Описание |
|-------|-----|----------|
| `GET` | `/api/orders` | Список заказов с пагинацией |
| `GET` | `/api/orders/{id}` | Получить заказ по ID |
| `GET` | `/api/orders/customer/{customerId}` | Заказы клиента |
| `GET` | `/api/orders/status/{status}` | Заказы по статусу |
| `POST` | `/api/orders` | Создать новый заказ |
| `PUT` | `/api/orders/{id}/status` | Изменить статус заказа |
| `DELETE` | `/api/orders/{id}` | Удалить заказ (только Created) |

#### 📝 Управление строками заказа:
| Метод | URL | Описание |
|-------|-----|----------|
| `POST` | `/api/orders/{id}/order-lines` | Добавить товар в заказ |
| `PUT` | `/api/orders/{id}/order-lines` | Изменить количество товара |
| `DELETE` | `/api/orders/{id}/order-lines/{lineId}` | Удалить товар из заказа |

## 🔄 Статусы заказов

- **Created** (1) - Создан
- **InProgress** (2) - В работе  
- **Completed** (3) - Завершен
- **Cancelled** (4) - Отменен

### Правила смены статусов:
- `Created` → `InProgress` или `Cancelled`
- `InProgress` → `Completed` или `Cancelled`
- `Completed` и `Cancelled` - финальные статусы

## 📝 Примеры использования

### Создание клиента
```bash
curl -X POST "https://localhost:7180/api/customers" \
-H "Content-Type: application/json" \
-d '{
  "name": "Иван",
  "lastName": "Петров",
  "address": {
    "street": "ул. Пушкина, д. 10",
    "city": "Москва",
    "state": "Московская область",
    "country": "Россия",
    "postalCode": "101000"
  },
  "photo": "https://example.com/photo.jpg"
}'
```

### Создание продукта
```bash
curl -X POST "https://localhost:7180/api/products" \
-H "Content-Type: application/json" \
-d '{
  "name": "Смартфон iPhone 15",
  "price": {
    "amount": 99999.99,
    "currency": "RUB"
  }
}'
```

### Создание заказа
```bash
curl -X POST "https://localhost:7180/api/orders" \
-H "Content-Type: application/json" \
-d '{
  "customerId": "{customer-guid}"
}'
```

### Добавление товара в заказ
```bash
curl -X POST "https://localhost:7180/api/orders/{order-id}/order-lines" \
-H "Content-Type: application/json" \
-d '{
  "productId": "{product-guid}",
  "count": 2
}'
```

## 🔍 Пагинация

Все списочные endpoints поддерживают пагинацию:

**Параметры:**
- `page` - номер страницы (по умолчанию: 1)
- `pageSize` - размер страницы (по умолчанию: 10, максимум: 100)

**Пример:**
```
GET /api/customers?page=2&pageSize=20
```

**Ответ:**
```json
{
  "items": [...],
  "totalCount": 150,
  "page": 2,
  "pageSize": 20,
  "totalPages": 8,
  "hasNextPage": true,
  "hasPreviousPage": true
}
```