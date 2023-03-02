CREATE TABLE available_items(
	available_item_id INT PRIMARY KEY IDENTITY(1,1),
	item_serial_number INT NOT NULL FOREIGN KEY REFERENCES item(item_serial_number),
	item_stock INT NOT NULL,
	stock_copy INT NOT NULL
)