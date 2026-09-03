# Retail Analytics Dashboard

Sistema de gestión y análisis de ventas para retail, construido con .NET 9 y Angular 22. Incluye dashboard analítico, gestión de productos, ventas y reportes de Business Intelligence.

## 🚀 Tecnologías

### Backend
- **.NET 9** - API REST
- **Entity Framework Core 9** - ORM
- **SQL Server** - Base de datos
- **Swagger** - Documentación de API
- **JWT** - Autenticación (pendiente)

### Frontend
- **Angular 22** - Framework SPA
- **TypeScript** - Lenguaje
- **RxJS** - Programación reactiva
- **Angular Material** - Componentes UI

## 🔧 Instalación

### Requisitos previos
- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 20+](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) o SQL Server Express

### Backend

```bash
# 1. Entrar a la carpeta del API
cd RetailAnalytics.API

# 2. Restaurar paquetes
dotnet restore

# 3. Crear la base de datos
dotnet ef database update

# 4. Ejecutar la API
dotnet run
La API estará disponible en: http://localhost:5063

Swagger: http://localhost:5063/swagger

Frontend
bash
# 1. Entrar a la carpeta del frontend
cd retail-analytics-ui

# 2. Instalar dependencias
npm install

# 3. Ejecutar Angular
ng serve
```
#📊 Endpoints de la API
Productos
```
GET    /api/products         - Obtener todos los productos
GET    /api/products/{id}    - Obtener producto por ID
POST   /api/products         - Crear nuevo producto
PUT    /api/products/{id}    - Actualizar producto
DELETE /api/products/{id}    - Eliminar producto (soft delete)
Ventas
text
GET    /api/sales            - Obtener todas las ventas
GET    /api/sales/{id}       - Obtener venta por ID
POST   /api/sales            - Crear nueva venta
GET    /api/sales/analytics  - Obtener analíticas de ventas
Parámetros de filtro
text
GET /api/sales?fromDate=2026-01-01&toDate=2026-12-31
GET /api/sales/analytics?fromDate=2026-01-01&toDate=2026-12-31

#📦 Modelos de Datos
Product
Campo	Tipo	Descripción
ProductId	int	ID único
Name	string	Nombre del producto
SKU	string	Código único
Price	decimal	Precio
StockQuantity	int	Cantidad en stock
IsActive	bool	Estado activo
Sale
Campo	Tipo	Descripción
SaleId	int	ID único
CustomerId	int	ID del cliente
TotalAmount	decimal	Total de la venta
Discount	decimal	Descuento aplicado
SaleDate	DateTime	Fecha de venta
Status	string	Estado de la venta
#🎨 Frontend
Páginas
Dashboard - Métricas principales (productos, ventas, ingresos)

Productos - Tabla con listado de productos

Ventas - Historial de ventas

Servicios
ApiService - Servicio base para HTTP

ProductsService - Operaciones de productos

SalesService - Operaciones de ventas
```
#🔒 Configuración de CORS
El backend está configurado para aceptar peticiones desde:

text
http://localhost:4200
#📝 Ejemplos de Uso
Crear una venta
```bash
curl -X POST http://localhost:5063/api/sales \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": 1,
    "paymentMethod": "CreditCard",
    "discount": 0,
    "saleItems": [
      {
        "productId": 1,
        "quantity": 2
      },
      {
        "productId": 3,
        "quantity": 1
      }
    ]
  }'
Crear una venta con PowerShell
powershell
$body = @{
    customerId = 1
    paymentMethod = "CreditCard"
    discount = 0
    saleItems = @(
        @{ productId = 1; quantity = 2 },
        @{ productId = 3; quantity = 1 }
    )
} | ConvertTo-Json -Depth 5

Invoke-RestMethod -Uri "http://localhost:5063/api/sales" -Method Post -Body $body -ContentType "application/json"
🗄️ Base de Datos
Migraciones
```

```
# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migración
dotnet ef database update

# Revertir migración
dotnet ef database update NombreMigracionAnterior

# Eliminar última migración
dotnet ef migrations remove
Datos Semilla
El proyecto incluye datos iniciales:

3 categorías

4 productos

2 clientes

#🚧 Próximas Mejoras
□ Autenticación JWT
□ Formularios CRUD completos
□ Gráficos con Chart.js
□ Exportación a Excel
□ Paginación en tablas
□ Tests unitarios
□ CI/CD pipeline
□ Despliegue a Azure/AWS
#📄 Licencia
Este proyecto es de uso educativo y demostrativo.

Desarrollado como proyecto demo para A3D Chile
```

## 2. **Hacer commit del README**

```bash
git add README.md
git commit -m "docs: Agregar README completo del proyecto"
git push origin main
