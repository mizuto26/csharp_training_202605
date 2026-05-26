TRUNCATE TABLE
    employee,
    department
RESTART IDENTITY CASCADE;

INSERT INTO department (id, name) OVERRIDING SYSTEM VALUE VALUES (1, '営業部');
INSERT INTO department (id, name) OVERRIDING SYSTEM VALUE VALUES (2, '総務部');
INSERT INTO department (id, name) OVERRIDING SYSTEM VALUE VALUES (3, '開発部');

INSERT INTO employee (id, name, phone_number, email_address, dept_id)
OVERRIDING SYSTEM VALUE VALUES (1, '山田', '03-1234-5678', 'yamada@example.com', 1);

INSERT INTO employee (id, name, phone_number, email_address, dept_id)
OVERRIDING SYSTEM VALUE VALUES (2, '鈴木', '090-1111-2222', 'suzuki@example.com', NULL);

SELECT setval(pg_get_serial_sequence('department', 'id'), (SELECT MAX(id) FROM department));
SELECT setval(pg_get_serial_sequence('employee', 'id'), (SELECT MAX(id) FROM employee));
