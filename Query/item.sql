CREATE TABLE item(
	item_serial_number INT PRIMARY KEY NOT NULL,
	item_name VARCHAR(255) NOT NULL,
	item_model VARCHAR(255) NOT NULL,
	item_warranty DATE NOT NULL,
	item_warranty_word VARCHAR NOT NULL,
	item_price FLOAT NOT NULL,
	item_description TEXT,
	item_added_date DATE NOT NULL
)