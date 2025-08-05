create table products (
	product_id serial primary key,
	product_name varchar(50) not null,
	product_price float not null
);

create table discounts (
	discount_id serial primary key,
	product_id serial,
	discount_percentage int not null,
	minimal_quantity int not null,
	constraint fk_product
		foreign key(product_id)
			references products(product_id)
			on delete cascade, -- Deleting entry if product no longer exists.
	constraint check_quantity_not_0
		check(minimal_quantity > 0),
	constraint check_percentage_not_0
		check(discount_percentage > 0)
);

create index discount_product_id_idx on discounts(product_id); -- Creating an index on the discounts.product_id for better joining.

create table orders (
	order_id serial primary key,
	order_name varchar(50) not null
);

create table order_lines (
	order_line_id serial primary key,
	order_id serial,
	product_id serial,
	product_quantity int not null,
	constraint fk_order
		foreign key(order_id)
			references orders(order_id)
			on delete cascade,
	constraint fk_product
		foreign key(product_id)
			references products(product_id)
			on delete cascade,
	constraint check_product_quantity_not_0
		check(product_quantity > 0)
);

create index orderlines_order_id_idx on orderlines(order_id); 
create index orderlines_product_id_idx on orderlines(product_id); 