CREATE TABLE vendor_details (
  vendor_id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
  vendor_name NVARCHAR(100) NOT NULL,
  country NVARCHAR(100) NULL,
  state NVARCHAR(100) NULL,
  city NVARCHAR(100) NULL,
  phone BIGINT NOT NULL UNIQUE,
  pin NVARCHAR(20) NULL,
  latitude DECIMAL(12,9) NULL,
  longitude DECIMAL(12,9) NULL,
  is_social_distance BIT DEFAULT 0,
  is_fever_screen BIT DEFAULT 0,
  is_sanitizer_used BIT DEFAULT 0,
  is_stamp_check BIT DEFAULT 0,
);
GO
CREATE TABLE category (
  category_id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
  category_name NVARCHAR(100) NOT NULL
);
GO
INSERT INTO [dbo].[category] VALUES('Fruits')
INSERT INTO [dbo].[category] VALUES('Vegetables')
GO
CREATE TABLE product (
  product_id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
  product_name NVARCHAR(500) NOT NULL,
  category_id [bigint] NOT NULL,
  image_path NVARCHAR(100) NULL
);
GO
CREATE TABLE vendor_category (
  vendor_category_id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
  category_id BIGINT NOT NULL,
  vendor_id BIGINT NOT NULL,
  is_active BIT NOT NULL
  --FOREIGN KEY (vendor_id) REFERENCES vendor_details(vendor_id),
  --FOREIGN KEY (category_id) REFERENCES category(category_id)
);
GO
CREATE TABLE vendor_product (
vendor_product_id BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
vendor_id BIGINT NOT NULL,
product_id BIGINT NOT NULL,
min_order_quantity DECIMAL(20,0) NULL,
price DECIMAL(20,2) NULL,
Units NVARCHAR(20) NULL,
--FOREIGN KEY (vendor_id) REFERENCES vendor_details(vendor_id),
--FOREIGN KEY (product_id) REFERENCES product(product_id),
);
GO
INSERT INTO product
VALUES ('Apple', 1, 'http://vendor.comwin.in/content/images/Apple.png'),
('Mango', 1, 'http://vendor.comwin.in/content/images/Mango.png'),
('Grapes Green Round', 1, 'http://vendor.comwin.in/content/images/Grapes_Green_Round.png'),
('Banana', 1, 'http://vendor.comwin.in/content/images/Banana.png'),
('Santra', 1, 'http://vendor.comwin.in/content/images/Santra.png'),
('Peru', 1, 'http://vendor.comwin.in/content/images/Peru.png'),
('Anaar', 1, 'http://vendor.comwin.in/content/images/Anaar.png'),
('KairiKacchi', 1, ''),
('Mossambi', 1, ''),
('Chikoo', 1, ''),
('Sitafal', 1, ''),
('Papaya', 1, ''),
('Fanas', 1, ''),
('Pineapple', 1, ''),
('Water Melon', 1, ''),
('Lychee', 1, ''),
('Strawberry', 1, ''),
('Aamla',1, ''),
('Plum', 1, ''),
('Peach',1, ''),
('Anjir', 1, ''),
('Kiwi', 1, ''),
('Grapes Green Long', 1, ''),
('Grapes Purple Round', 1, ''),
('Melon Kharbuj', 1, ''),
('Tomato', 2, ''),
('Lemon', 2, ''),
('Peas', 2, ''),
('Doodhi', 1, ''),
('Karela', 2, ''),
('Turai', 2, '')
GO

ALTER TABLE vendor_details
ADD contactless_pay BIT NULL;
GO