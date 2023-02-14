CREATE TABLE item(
	item_serial_number INT PRIMARY KEY IDENTITY(1,1),
	item_name VARCHAR(255) NOT NULL,
	item_price FLOAT NOT NULL,
	item_description TEXT,
	item_model VARCHAR(255) NOT NULL,
	item_warranty INT NOT NULL,
	item_added_date DATE NOT NULL
)