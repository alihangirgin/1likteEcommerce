  ## Proje Mimarisi
Basit bir N-Tier mimari kullandım, projede 4 katman bulunmaktadır.
- 1likteEcommerce.Business Katmanı (Business Logic)
Veri tabanından verileri alarak gerekli business işlemlerini yapıp sunum katmanına gönderir veya sunum katmanından gelen istekleri için gerekli business işlemlerini yapıp veri tabanına gönderir. Verileri Core katmanı vasıtasıyla alır veya gönderir, bu yüzden bu katmana bağımlılığı vardır.
- 1likteEcommerce.Core Katmanı(Infrastructure) 
Projenin altyapı katmanıdır. Interface ve Entity model yapıları bu katmanda tanımlanır ve tanımlanan Dto nesneleri ile katmanlar arası veri aktarımını sağlar. 
- 1likteEcommerce.Data Katmanı (Data Access)
Veritabanı işlemlerinden sorumlu katmandır, bu nedenle veritabanı ile ilgili classlar bu katmanda tanımlanır. Veri tabanı işlemlerinde RepositoryPattern ve UnitOfWork kullandım. Verileri Core katmanı vasıtasıyla alır veya gönderir, bu yüzden bu katmana bağımlılığı vardır.
- 1likteEcommerce.Api Katmanı (Presentation)
Api controllerların sunum katmanıdır. Api'dan gelen requestler göre Business katmanına Core katmanı vasıtasıyla veri gönderir veya alır.
### Data Flow
- Presentation (1likteEcommerce.Api) -> Business (1likteEcommerce.Business) -> Data Access (1likteEcommerce.Data) --> Database
- Presentation (1likteEcommerce.Api) <- Business (1likteEcommerce.Business) <- Data Access (1likteEcommerce.Data) <-- Database
### Database Design
Veritabanı oluşturulurken Entity Framework Code-First yaklaşımı kullanıldı. Repository Pattern olabildiğince Generic yapılmaya çalışıldı. Crud işlemleri UnitOfWork class üzerinden yapıldı.
- DbContext-> 1likteEcommerce.Data.DataAccess
- Models(Entities) -> 1likteEcommerce.Core.Models
- Repository-> 1likteEcommerce.Data.Repositories
- IRepositoy-> 1likteEcommerce.Core.Repositories
- UnitOfWork->1likteEcommerce.Data.UnitOfWork
- IUnitOfWork->1likteEcommerce.Core.UnitOfWork
### Entities
- Basket 
- BasketItem 
- Category 
- Product
- User (.Net Identity)
### Entity Relationships
- User 1-1 Basket
- Basket 1-n BasketItem
- Category 1-n Product
- BasketItem 1-n Product

### Endpointler:

### UserController Controller ("api/users")
- User/Register-> to register, POST ("api/users/register")
- User/Login-> to login, POST ("api/users/login")
- User/UpdateUser-> to update an user, PUT ("api/users/{userId}")
- User/GetUsers-> to list all users, GET ("api/users")
- User/UploadProfilePhoto-> to upload profile photo, POST ("api/users/{userId}/upload-photo")
- User/DownloadProfilePhoto-> to download uploaded profile photo, GET ("api/users/{userId}/download-photo")
### Basket Controller ("api/baskets")
- Basket/AddProductToBasket -> to add product to basket, POST ("api/baskets/add-product")
- Basket/RemoveProductFromBasket -> to remove product from basket, POST ("api/baskets/remove-product")
- Basket/GetBasket -> to get user basket, GET ("api/baskets")
### Category Controller ("api/categories")
- Category/CreateCategory -> to create a new category, POST ("api/categories")
- Category/UpdateCategory -> to update a category, PUT ("api/categories/{categoryId}")
- Category/DeleteCategory -> to delete a category, DELETE ("api/categories/{categoryId}")
- Category/GetCategory -> to get a specific category, GET ("api/categories/{categoryId}")
- Category/GetCategories -> to list all categories, GET ("api/categories")
### Product Controller ("api/products")
- Product/CreateProduct -> to create a new product, POST ("api/products")
- Product/UpdateProduct -> to update a product, PUT ("api/products/{productId}")
- Product/DeleteProduct -> to delete a product, DELETE ("api/products/{productId}")
- Product/GetProduct -> to get a specific product, GET ("api/products/{productId}")
- Product/GetProducts -> to list all products GET, ("api/products")

### Authorization
Login isteği ile Bearer Token alınıp, istekler Bearer Token ile gönderilmelidir. Bearer Token olarak JWT kullandım.

### Unit Test
Xunit ve Moq kütüphanesi kullanım. Unit Test class'lar 1likteEcommerce.Api.UnitTests içerisinde

<h2>Docker</h2>

PostgreDb Docker'da ayakta olmalıdır.

![image](https://github.com/user-attachments/assets/1638a5a0-588f-45f0-91c4-fbf5dec416c0)

Docker'da Postgre container oluşturmak için aşağıdaki komut kullanılır. Appsetting db host postgresdb olarak verildiği için localde normal halde çalıştırmak için appsettings tekrar localhosta çekilmeli.

- docker run --name postgresdb -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=yourpassword -e POSTGRES_DB=1likteTest5 -p 5432:5432 -d postgres

  ![image](https://github.com/user-attachments/assets/14126fec-98e5-4964-8160-047530640ee8)

Uygulama container ve Postgre container arasında iletişim sağlamak için aynı ağ üzerinde olmaları gerekti. Aşağıdaki komutlarla mynetwork isminde bir ağ oluşturdum ve Postgre container'ı ekledim

- docker network create mynetwork
- docker network connect mynetwork postgresdb

Uygulama container'ı ayağa kaldırmak için aşağıdaki komutları kullandım. Açtığım mynetwork ağını vermek gerekiyor.

- cd {yourFolderPath}\1likteEcommerce
- docker build -t 1likteecommerceapi -f 1likteEcommerce.Api/Dockerfile .
- docker run -d --name 1likteecommerceapi --network mynetwork -p 5000:80 1likteecommerceapi

Dockerfile'da 80 portunu expose ettiğim için 80'de başlattım. Yine ssl sertifika kurulu olmadığı için development mode'da başlattım.

![image](https://github.com/user-attachments/assets/371dbe63-cd2e-47fb-bede-342721ee764f)

File upload için dosya okuma/yazma işlemlerinde yetkinlendirme gerektiği için DockerFile'da root user'ına geçtim ve geri döndüm. Kendi docker userınız varsa "USER 1000:1000" yerine verebilirsiniz 

![image](https://github.com/user-attachments/assets/eb87f75c-85a9-4284-8cfc-2335758a75b3)


Docker app url: http://localhost:5000/index.html

![image](https://github.com/user-attachments/assets/4e1904ed-2fe2-4bfd-8ec7-f7c1065dda2e)

<h2>Swagger</h2>

Swagger'a prefix vermedim, uygulama Url'ine gidildiğinde otomatik yönlendirilecek. SSl'den dolayı Docker container'ı dev modunda başlattığım için dev modda da swagger'ı açık bıraktım.

![image](https://github.com/user-attachments/assets/832165f2-beba-45da-a867-fbb41ef14b37)

## Todo
 - Exception Handling
 - Logging
 - Base Api Response Model
