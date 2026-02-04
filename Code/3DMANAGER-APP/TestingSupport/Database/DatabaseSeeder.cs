namespace _3DMANAGER_APP.TestingSupport.Database
{
    public class DatabaseSeeder
    {
        private readonly string _connectionString;

        public DatabaseSeeder(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("ConnectionString inválida");

            if (!connectionString.Contains("3dmanagerCI", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(
                    "DatabaseSeeder SOLO puede ejecutarse contra la BBDD de CI/Test"
                );

            _connectionString = connectionString;
        }

        public async Task SeedAsync()
        {
            await CreateSchemaAsync();
            await CreateStoredProceduresAsync();
            await LoadDataAsync();
        }

        private async Task CreateSchemaAsync()
        {
            await CreateGroupTableAsync();
            await CreateCatalogTablesAsync();
            await CreateUserTableAsync();
            await CreatePrinterTableAsync();
            await CreateFilamentTableAsync();
            await Create3DPrintTableAsync();
        }
        private async Task CreateStoredProceduresAsync()
        {
            await CreateProcCFilamentAsync();
            await CreateProcCPrintStateAsync();
            await CreateProcCPrinterAsync();
            await CreateProcCFilamentTypeAsync();
            await CreateProcFilamentListAsync();
            await CreateProcFilamentPostAsync();
            await CreateProcPrintListAsync();
            await CreateProcPrintPostAsync();
            await CreateProcPrinterListAsync();
            await CreateProcPrinterPostAsync();
            await CreateProcUserListAsync();
            await CreateProcUserLoginAsync();
            await CreateProcUserPostAsync();
        }

        private async Task LoadDataAsync()
        {
            var sql = """
            INSERT INTO `3DMANAGER_C_STATE_PRINTER`
            (3DMANAGER_C_STATE_PRINTER_NAME)
            VALUES ('Activo');

            INSERT INTO `3DMANAGER_C_STATE_FILAMENT`
            (3DMANAGER_C_STATE_FILAMENT_NAME)
            VALUES ('Disponible');

            INSERT INTO `3DMANAGER_C_ROLES`
            (3DMANAGER_C_ROLES_NAME)
            VALUES ('Usuario-Base'),('Usuario-Manager'),('Usuario-Invitado');

            INSERT INTO `3DMANAGER_C_TYPE_FILAMENT`
            (3DMANAGER_C_TYPE_FILAMENT_NAME)
            VALUES ('PLA'),('PLA +');

            INSERT INTO `3DMANAGER_C_STATE_PRINT`
            (3DMANAGER_C_STATE_PRINT_NAME)
            VALUES ('Pendiente'),('Completada');

            INSERT INTO `3DMANAGER_GROUP`
            (3DMANAGER_GROUP_NAME, 3DMANAGER_GROUP_DESCRIPTION)
            VALUES
            ('Grupo Test', 'Grupo de pruebas CI');

            INSERT INTO `3DMANAGER_USER`
            (USER_NAME,USER_PASSWORD,USER_EMAIL, USER_ROLE, USER_GROUP_ID)
            VALUES
            ('user_test','AQAAAAIAAYagAAAAENxwolbYGMFDoMUv/KEjKYtH7Vg1yQ3J5BKFMYp30ZrCXD5Xz0bxofJaat+FUBgCDQ==','user@test.com',2,1);

            INSERT INTO `3DMANAGER_PRINTER`
            (3DMANAGER_PRINTER_NAME, 3DMANAGER_PRINTER_MODEL, 3DMANAGER_PRINTER_TIME_VARIATION, 3DMANAGER_PRINTER_STATE,
            3DMANAGER_PRINTER_GROUP_ID)
            VALUES
            ('Printer CI','Ender 3', 1.15,1,1),
            ('Printer2 CI','Ender 4', 0.85,1,1);

            INSERT INTO `3DMANAGER_FILAMENT`
            (
                3DMANAGER_FILAMENT_NAME,
                3DMANAGER_FILAMENT_DESCRIPTION,
                3DMANAGER_FILAMENT_STATE,
                3DMANAGER_FILAMENT_MATERIAL_LENGTH,
                3DMANAGER_FILAMENT_MATERIAL_REMAINING_LENGTH,
                3DMANAGER_FILAMENT_MATERIAL_THICKNESS,
                3DMANAGER_FILAMENT_TEMPERATURE,
                3DMANAGER_FILAMENT_COLOR,
                3DMANAGER_FILAMENT_GROUP_ID,
                3DMANAGER_FILAMENT_MATERIAL_TYPE,
                3DMANAGER_FILAMENT_COST
            )
            VALUES
            ('PLA Negro','Filamento de prueba',1,1000,800,1.75,200,'000000',1,1,10),
            ('PLA Blanco','Filamento de prueba',1,1000,800,1.75,200,'000000',1,1,20);

            INSERT INTO `3DMANAGER_3DPRINT`
            (
                3DMANAGER_3DPRINT_NAME,
                3DMANAGER_3DPRINT_STATE,
                3DMANAGER_3DPRINT_IMPRESSION_TIME,
                3DMANAGER_3DPRINT_REAL_IMPRESSION_TIME,
                3DMANAGER_3DPRINT_MATERIAL_CONSUMED,
                3DMANAGER_3DPRINT_GROUP_ID,
                3DMANAGER_3DPRINT_PRINTER_ID,
                3DMANAGER_3DPRINT_USER_ID,
                3DMANAGER_3DPRINT_FILAMENT_ID,
                3DMANAGER_3DPRINT_DESCRIPTION
            )
            VALUES
            ('Pieza Test CI',1,120,130,15.75,1,1,1,1,'Impresión de prueba'),
            ('Pieza Test 2 CI',1,120,130,15.75,1,2,2,1,'Impresión de prueba 2'),
            ('Pieza Test 3 CI',1,120,130,15.75,1,2,2,2,'Impresión de prueba 3');

            """;



            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        public async Task CleanupAsync()
        {
            var sql = """
                DROP TABLE IF EXISTS `3DMANAGER_3DPRINT`;
                DROP TABLE IF EXISTS `3DMANAGER_FILAMENT`;
                DROP TABLE IF EXISTS `3DMANAGER_PRINTER`;
                DROP TABLE IF EXISTS `3DMANAGER_USER`;
                DROP TABLE IF EXISTS `3DMANAGER_C_TYPE_NOTIFICATION`;
                DROP TABLE IF EXISTS `3DMANAGER_C_STATE_PRINT`;
                DROP TABLE IF EXISTS `3DMANAGER_C_TYPE_FILAMENT`;
                DROP TABLE IF EXISTS `3DMANAGER_C_ROLES`;
                DROP TABLE IF EXISTS `3DMANAGER_C_STATE_FILAMENT`;
                DROP TABLE IF EXISTS `3DMANAGER_C_STATE_PRINTER`;
                DROP TABLE IF EXISTS `3DMANAGER_GROUP`;
                """;
            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateGroupTableAsync()
        {
            var sql = """
            CREATE TABLE IF NOT EXISTS `3DMANAGER_GROUP` (
                `3DMANAGER_GROUP_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_GROUP_NAME` VARCHAR(100) NOT NULL,
                `3DMANAGER_GROUP_DESCRIPTION` VARCHAR(500),
                `3DMANAGER_PRINTER_REGISTER_DATE` DATETIME DEFAULT CURRENT_TIMESTAMP
            );
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateCatalogTablesAsync()
        {
            var sql = """
            CREATE TABLE IF NOT EXISTS `3DMANAGER_C_STATE_PRINTER` (
                `3DMANAGER_C_STATE_PRINTER_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_C_STATE_PRINTER_NAME` VARCHAR(50) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS `3DMANAGER_C_STATE_FILAMENT` (
                `3DMANAGER_C_STATE_FILAMENT_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_C_STATE_FILAMENT_NAME` VARCHAR(50) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS `3DMANAGER_C_ROLES` (
                `3DMANAGER_C_ROLES_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_C_ROLES_NAME` VARCHAR(30) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS `3DMANAGER_C_TYPE_FILAMENT` (
                `3DMANAGER_C_TYPE_FILAMENT_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_C_TYPE_FILAMENT_NAME` VARCHAR(30) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS `3DMANAGER_C_STATE_PRINT` (
                `3DMANAGER_C_STATE_PRINT_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_C_STATE_PRINT_NAME` VARCHAR(50) NOT NULL
            );

            CREATE TABLE IF NOT EXISTS `3DMANAGER_C_TYPE_NOTIFICATION` (
                `3DMANAGER_C_TYPE_NOTIFICATION_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_C_TYPE_NOTIFICATION_NAME` VARCHAR(50)
            );
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateUserTableAsync()
        {
            var sql = """
            CREATE TABLE IF NOT EXISTS `3DMANAGER_USER` (
                `USER_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `USER_NAME` VARCHAR(100) NOT NULL,
                `USER_PASSWORD` VARCHAR(100) NOT NULL,
                `USER_EMAIL` VARCHAR(100) NOT NULL,
                `USER_ROLE` INT NULL,
                `USER_GROUP_ID` INT NOT NULL,
                `USER_PHOTO_URL` VARCHAR(255),
                `USER_REGISTER_DATE` DATETIME DEFAULT CURRENT_TIMESTAMP,
                `USER_IMAGE_URL` varchar(255) DEFAULT NULL,
                `USER_IMAGE_KEY` varchar(255) DEFAULT NULL,
                FOREIGN KEY (`USER_GROUP_ID`)
                    REFERENCES `3DMANAGER_GROUP` (`3DMANAGER_GROUP_ID`)
            );
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreatePrinterTableAsync()
        {
            var sql = """
            CREATE TABLE IF NOT EXISTS `3DMANAGER_PRINTER` (
                `3DMANAGER_PRINTER_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_PRINTER_NAME` VARCHAR(100) NOT NULL,
                `3DMANAGER_PRINTER_MODEL` VARCHAR(100),
                `3DMANAGER_PRINTER_TIME_VARIATION` FLOAT NULL,
                `3DMANAGER_PRINTER_STATE` INT NULL,
                `3DMANAGER_PRINTER_GROUP_ID` INT NULL,
                `3DMANAGER_PRINTER_REGISTER_DATE` DATETIME DEFAULT CURRENT_TIMESTAMP,
                `3DMANAGER_PRINTER_DESCRIPTION` varchar(100) DEFAULT NULL,
                `3DMANAGER_PRINTER_IMAGE_URL` varchar(255) DEFAULT NULL,
                `3DMANAGER_PRINTER_IMAGE_KEY` varchar(255) DEFAULT NULL,
                FOREIGN KEY (`3DMANAGER_PRINTER_GROUP_ID`)
                    REFERENCES `3DMANAGER_GROUP` (`3DMANAGER_GROUP_ID`)
            );
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateFilamentTableAsync()
        {
            var sql = """
            CREATE TABLE IF NOT EXISTS `3DMANAGER_FILAMENT` (
                `3DMANAGER_FILAMENT_ID` INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_FILAMENT_NAME` VARCHAR(100) NOT NULL,
                `3DMANAGER_FILAMENT_DESCRIPTION` VARCHAR(500) DEFAULT NULL,
                `3DMANAGER_FILAMENT_STATE` INT NOT NULL,
                `3DMANAGER_FILAMENT_MATERIAL_LENGTH` DECIMAL(10,4) NOT NULL,
                `3DMANAGER_FILAMENT_MATERIAL_REMAINING_LENGTH` DECIMAL(10,4) NOT NULL,
                `3DMANAGER_FILAMENT_MATERIAL_THICKNESS` FLOAT DEFAULT NULL,
                `3DMANAGER_FILAMENT_TEMPERATURE` INT DEFAULT NULL,
                `3DMANAGER_FILAMENT_COLOR` VARCHAR(10) DEFAULT NULL,
                `3DMANAGER_FILAMENT_GROUP_ID` INT NOT NULL,
                `3DMANAGER_FILAMENT_MATERIAL_TYPE` INT NOT NULL,
                `3DMANAGER_FILAMENT_REGISTER_DATE` DATETIME DEFAULT CURRENT_TIMESTAMP,
                `3DMANAGER_FILAMENT_WEIGHT` INT DEFAULT NULL,
                `3DMANAGER_FILAMENT_COST` DECIMAL(10,2) DEFAULT NULL,
                `3DMANAGER_FILAMENT_IMAGE_URL` varchar(255) DEFAULT NULL,
                `3DMANAGER_FILAMENT_IMAGE_KEY` varchar(255) DEFAULT NULL,
                FOREIGN KEY (`3DMANAGER_FILAMENT_GROUP_ID`)
                    REFERENCES `3DMANAGER_GROUP` (`3DMANAGER_GROUP_ID`)
            );
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task Create3DPrintTableAsync()
        {
            var sql = """
            CREATE TABLE IF NOT EXISTS `3DMANAGER_3DPRINT` (
                `3DMANAGER_3DPRINT_ID` INT AUTO_INCREMENT PRIMARY KEY,
                `3DMANAGER_3DPRINT_NAME` VARCHAR(100) NOT NULL,
                `3DMANAGER_3DPRINT_STATE` INT NOT NULL,
                `3DMANAGER_3DPRINT_IMPRESSION_TIME` INT NOT NULL,
                `3DMANAGER_3DPRINT_REAL_IMPRESSION_TIME` INT NOT NULL,
                `3DMANAGER_3DPRINT_MATERIAL_CONSUMED` DECIMAL(10,2) NOT NULL,
                `3DMANAGER_3DPRINT_GROUP_ID` INT NOT NULL,
                `3DMANAGER_3DPRINT_PRINTER_ID` INT NOT NULL,
                `3DMANAGER_3DPRINT_USER_ID` INT NOT NULL,
                `3DMANAGER_3DPRINT_FILAMENT_ID` INT NOT NULL,
                `3DMANAGER_3DPRINT_DESCRIPTION` VARCHAR(500),
                `3DMANAGER_3DPRINT_REGISTER_DATE` DATETIME DEFAULT CURRENT_TIMESTAMP,
                `3DMANAGER_3DPRINT_IMAGE_KEY` varchar(255) DEFAULT NULL,
                `3DMANAGER_3DPRINT_IMAGE_URL` varchar(255) DEFAULT NULL
            );
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcCFilamentAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_C_FILAMENT`;

            CREATE PROCEDURE `3DMANAGER_pr_C_FILAMENT`(
                IN P_CD_GROUP INT,
                OUT CodigoError INT
            )
            BEGIN
                SET CodigoError = 0;

                SELECT 
                    `3DMANAGER_FILAMENT_ID` AS ID,
                    `3DMANAGER_FILAMENT_NAME` AS DESCRIPTION
                FROM `3DMANAGER_FILAMENT`
                WHERE `3DMANAGER_FILAMENT_GROUP_ID` = P_CD_GROUP
                  AND `3DMANAGER_FILAMENT_STATE` = 1;
            END;
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcUserPostAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_USER_POST`;

            CREATE PROCEDURE `3DMANAGER_pr_USER_POST`(
                IN P_DS_USER_NAME VARCHAR(100),
                IN P_DS_USER_PASSWORD VARCHAR(255),
                IN P_DS_USER_EMAIL VARCHAR(100),
                OUT CodigoError INT
            )
            BEGIN
                SET CodigoError = 0;

                INSERT INTO `3DMANAGER_USER`
                (
                    `USER_NAME`,
                    `USER_PASSWORD`,
                    `USER_EMAIL`
                )
                VALUES
                (
                    P_DS_USER_NAME,
                    P_DS_USER_PASSWORD,
                    P_DS_USER_EMAIL
                );

                SELECT LAST_INSERT_ID() AS USER_ID;
            END;
            
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcUserLoginAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_USER_LOGIN`;

            CREATE PROCEDURE `3DMANAGER_pr_USER_LOGIN`(
                IN P_DS_USER_NAME VARCHAR(100)
            )
            BEGIN
                SELECT 
                    u.`USER_ID`,
                    u.`USER_NAME`,
                    u.`USER_PASSWORD`,
                    u.`USER_EMAIL`,
                    u.`USER_GROUP_ID`,
                    r.`3DMANAGER_C_ROLES_NAME` AS USER_ROLE,
                    g.`3DMANAGER_GROUP_NAME` AS GROUP_NAME
                FROM `3DMANAGER_USER` u
                LEFT JOIN `3DMANAGER_C_ROLES` r
                    ON u.`USER_ROLE` = r.`3DMANAGER_C_ROLES_ID`
                LEFT JOIN `3DMANAGER_GROUP` g
                    ON g.`3DMANAGER_GROUP_ID` = u.`USER_GROUP_ID`
                WHERE u.`USER_NAME` = P_DS_USER_NAME;
            END;
            
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcUserListAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_USER_LIST`;

            CREATE PROCEDURE `3DMANAGER_pr_USER_LIST`(
                IN P_CD_GROUP INT,
                OUT CodigoError INT
            )
            BEGIN
                SET CodigoError = 0;

                SELECT 
                    `USER_ID`,
                    `USER_NAME`,
                    IFNULL(
                        (
                            SELECT SUM(`3DMANAGER_3DPRINT_IMPRESSION_TIME`)
                            FROM `3DMANAGER_3DPRINT`
                            WHERE `3DMANAGER_3DPRINT_GROUP_ID` = P_CD_GROUP
                              AND `3DMANAGER_3DPRINT_USER_ID` = `USER_ID`
                        ), 0
                    ) AS USER_HOURS,
                    (
                        SELECT COUNT(*)
                        FROM `3DMANAGER_3DPRINT`
                        WHERE `3DMANAGER_3DPRINT_GROUP_ID` = P_CD_GROUP
                          AND `3DMANAGER_3DPRINT_USER_ID` = `USER_ID`
                    ) AS NUMBER_PRINTS
                FROM `3DMANAGER_USER`
                WHERE `USER_GROUP_ID` = P_CD_GROUP;
            END;
            
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcPrinterPostAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_PRINTER_POST`;

            CREATE PROCEDURE `3DMANAGER_pr_PRINTER_POST`(
                IN P_GROUP_ID INT,
                IN P_PRINTER_NAME VARCHAR(100),
                IN P_PRINTER_MODEL VARCHAR(100),
                IN P_PRINTER_DESCRIPTION VARCHAR(500),
                OUT CodigoError INT
            )
            BEGIN
                DECLARE NEW_ID INT;
                SET CodigoError = 0;

                INSERT INTO `3DMANAGER_PRINTER` (
                    `3DMANAGER_PRINTER_NAME`,
                    `3DMANAGER_PRINTER_DESCRIPTION`,
                    `3DMANAGER_PRINTER_STATE`,
                    `3DMANAGER_PRINTER_MODEL`,
                    `3DMANAGER_PRINTER_GROUP_ID`
                )
                VALUES (
                    P_PRINTER_NAME,
                    P_PRINTER_DESCRIPTION,
                    1,
                    P_PRINTER_MODEL,
                    P_GROUP_ID
                );

                SET NEW_ID = LAST_INSERT_ID(); 
                SELECT NEW_ID AS 3DMANAGER_PRINTER_ID;
            END;
            
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcPrinterListAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_PRINTER_LIST`;

            CREATE PROCEDURE `3DMANAGER_pr_PRINTER_LIST`(
                IN P_CD_GROUP INT,
                OUT CodigoError INT
            )
            BEGIN
                SET CodigoError = 0;

                SELECT 
                    p.`3DMANAGER_PRINTER_ID` AS PRINTER_ID,
                    p.`3DMANAGER_PRINTER_NAME` AS PRINTER_NAME,
                    p.`3DMANAGER_PRINTER_MODEL` AS PRINTER_MODEL,
                    p.`3DMANAGER_PRINTER_DESCRIPTION` AS PRINTER_DESCRIPTION,
                    p.`3DMANAGER_PRINTER_STATE` AS PRINTER_STATE_ID,
                    s.`3DMANAGER_C_STATE_PRINTER_NAME` AS PRINTER_STATE_NAME,
                    p.`3DMANAGER_PRINTER_IMAGE_URL` AS FILE_URL,
                    p.`3DMANAGER_PRINTER_IMAGE_KEY` AS FILE_KEY
                FROM `3DMANAGER_PRINTER` p
                LEFT JOIN `3DMANAGER_C_STATE_PRINTER` s
                    ON s.`3DMANAGER_C_STATE_PRINTER_ID` = p.`3DMANAGER_PRINTER_STATE`
                WHERE p.`3DMANAGER_PRINTER_GROUP_ID` = P_CD_GROUP;
            END;
            
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcPrintPostAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_PRINT_POST`;

            CREATE PROCEDURE `3DMANAGER_pr_PRINT_POST`(
                IN P_GROUP_ID INT,
                IN P_USER_ID INT,
                IN P_PRINT_NAME VARCHAR(100),
                IN P_PRINT_STATE INT,
                IN P_PRINT_PRINTER_ID INT,
                IN P_PRINT_FILAMENT_ID INT,
                IN P_PRINT_DESCRIPTION VARCHAR(500),
                IN P_PRINT_TIME INT,
                IN P_PRINT_FILAMENT_USED DECIMAL(10,2),
                IN P_PRINT_REAL_TIME INT,
                OUT CodigoError INT
            )
            BEGIN
                DECLARE NEW_ID INT;
                SET CodigoError = 0;

                INSERT INTO `3DMANAGER_3DPRINT` (
                    `3DMANAGER_3DPRINT_NAME`,
                    `3DMANAGER_3DPRINT_DESCRIPTION`,
                    `3DMANAGER_3DPRINT_STATE`,
                    `3DMANAGER_3DPRINT_IMPRESSION_TIME`,
                    `3DMANAGER_3DPRINT_REAL_IMPRESSION_TIME`,
                    `3DMANAGER_3DPRINT_GROUP_ID`,
                    `3DMANAGER_3DPRINT_FILAMENT_ID`,
                    `3DMANAGER_3DPRINT_USER_ID`,
                    `3DMANAGER_3DPRINT_PRINTER_ID`,
                    `3DMANAGER_3DPRINT_MATERIAL_CONSUMED`
                )
                VALUES (
                    P_PRINT_NAME,
                    P_PRINT_DESCRIPTION,
                    P_PRINT_STATE,
                    P_PRINT_TIME,
                    P_PRINT_REAL_TIME,
                    P_GROUP_ID,
                    P_PRINT_FILAMENT_ID,
                    P_USER_ID,
                    P_PRINT_PRINTER_ID,
                    P_PRINT_FILAMENT_USED
                );

                SET NEW_ID = LAST_INSERT_ID();

                SELECT NEW_ID AS 3DMANAGER_3DPRINT_ID;
            END;
            
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcPrintListAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_PRINT_LIST`;

            CREATE PROCEDURE `3DMANAGER_pr_PRINT_LIST`(
                IN P_CD_GROUP INT,
                OUT CodigoError INT
            )
            BEGIN
                SET CodigoError = 0;

                SELECT 
                    p.`3DMANAGER_3DPRINT_ID` AS PRINT_ID,
                    p.`3DMANAGER_3DPRINT_NAME` AS PRINT_NAME,
                    u.`USER_NAME` AS PRINT_USER,
                    p.`3DMANAGER_3DPRINT_REGISTER_DATE` AS PRINT_DATE,
                    p.`3DMANAGER_3DPRINT_IMPRESSION_TIME` AS PRINT_TIME,
                    p.`3DMANAGER_3DPRINT_MATERIAL_CONSUMED` AS PRINT_FILAMENT_USED
                FROM `3DMANAGER_3DPRINT` p
                LEFT JOIN `3DMANAGER_USER` u
                    ON p.`3DMANAGER_3DPRINT_USER_ID` = u.`USER_ID`
                WHERE p.`3DMANAGER_3DPRINT_GROUP_ID` = P_CD_GROUP;
            END;
            
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcFilamentPostAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_FILAMENT_POST`;

            CREATE PROCEDURE `3DMANAGER_pr_FILAMENT_POST`(
                IN P_GROUP_ID INT,
                IN P_FILAMENT_NAME VARCHAR(100),
                IN P_FILAMENT_TYPE INT,
                IN P_FILAMENT_WEIGHT INT,
                IN P_FILAMENT_COLOR VARCHAR(10),
                IN P_FILAMENT_TEMPERATURE INT,
                IN P_FILAMENT_THICKNESS FLOAT,
                IN P_FILAMENT_COST DECIMAL(10,2),
                IN P_FILAMENT_LENGHT INT,
                IN P_FILAMENT_DESCRIPTION VARCHAR(500),
                OUT CodigoError INT
            )
            BEGIN
                DECLARE NEW_ID INT;
                SET CodigoError = 0;

                INSERT INTO `3DMANAGER_FILAMENT` (
                    `3DMANAGER_FILAMENT_NAME`,
                    `3DMANAGER_FILAMENT_DESCRIPTION`,
                    `3DMANAGER_FILAMENT_STATE`,
                    `3DMANAGER_FILAMENT_MATERIAL_LENGTH`,
                    `3DMANAGER_FILAMENT_MATERIAL_REMAINING_LENGTH`,
                    `3DMANAGER_FILAMENT_MATERIAL_THICKNESS`,
                    `3DMANAGER_FILAMENT_TEMPERATURE`,
                    `3DMANAGER_FILAMENT_COLOR`,
                    `3DMANAGER_FILAMENT_GROUP_ID`,
                    `3DMANAGER_FILAMENT_MATERIAL_TYPE`,
                    `3DMANAGER_FILAMENT_WEIGHT`,
                    `3DMANAGER_FILAMENT_COST`
                ) VALUES (
                    P_FILAMENT_NAME,
                    P_FILAMENT_DESCRIPTION,
                    1,
                    P_FILAMENT_LENGHT,
                    P_FILAMENT_LENGHT,
                    P_FILAMENT_THICKNESS,
                    P_FILAMENT_TEMPERATURE,
                    P_FILAMENT_COLOR,
                    P_GROUP_ID,
                    P_FILAMENT_TYPE,
                    P_FILAMENT_WEIGHT,
                    P_FILAMENT_COST
                );

                SET NEW_ID = LAST_INSERT_ID(); 
            	SELECT NEW_ID AS 3DMANAGER_FILAMENT_ID;
            END;
            
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcFilamentListAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_FILAMENT_LIST`;

            CREATE PROCEDURE `3DMANAGER_pr_FILAMENT_LIST`(
                IN P_CD_GROUP INT,
                OUT CodigoError INT
            )
            BEGIN
                SET CodigoError = 0;

                SELECT 
                    f.`3DMANAGER_FILAMENT_ID` AS FILAMENT_ID,
                    f.`3DMANAGER_FILAMENT_NAME` AS FILAMENT_NAME,
                    s.`3DMANAGER_C_STATE_FILAMENT_NAME` AS FILAMENT_STATE,
                    f.`3DMANAGER_FILAMENT_MATERIAL_REMAINING_LENGTH` AS FILAMENT_LENGTH,
                    f.`3DMANAGER_FILAMENT_COST` AS FILAMENT_COST
                FROM `3DMANAGER_FILAMENT` f
                LEFT JOIN `3DMANAGER_C_STATE_FILAMENT` s
                    ON f.`3DMANAGER_FILAMENT_STATE` = s.`3DMANAGER_C_STATE_FILAMENT_ID`
                WHERE f.`3DMANAGER_FILAMENT_GROUP_ID` = P_CD_GROUP;
            END;
            
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcCFilamentTypeAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_C_FILAMENT_TYPE`;

            CREATE PROCEDURE `3DMANAGER_pr_C_FILAMENT_TYPE`()
            BEGIN
                SELECT 
                    `3DMANAGER_C_TYPE_FILAMENT_ID` AS ID,
                    `3DMANAGER_C_TYPE_FILAMENT_NAME` AS DESCRIPTION
                FROM `3DMANAGER_C_TYPE_FILAMENT`;
            END;
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcCPrinterAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_C_PRINTER`;

            CREATE PROCEDURE `3DMANAGER_pr_C_PRINTER`(
                IN P_CD_GROUP INT,
                OUT CodigoError INT
            )
            BEGIN
                SET CodigoError = 0;

                SELECT 
                    `3DMANAGER_PRINTER_ID` AS ID,
                    `3DMANAGER_PRINTER_NAME` AS DESCRIPTION
                FROM `3DMANAGER_PRINTER`
                WHERE `3DMANAGER_PRINTER_GROUP_ID` = P_CD_GROUP
                  AND `3DMANAGER_PRINTER_STATE` = 1;
            END;
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }

        private async Task CreateProcCPrintStateAsync()
        {
            var sql = """
            DROP PROCEDURE IF EXISTS `3DMANAGER_pr_C_PRINT_STATE`;

            CREATE PROCEDURE `3DMANAGER_pr_C_PRINT_STATE`(
                OUT CodigoError INT
            )
            BEGIN
                SET CodigoError = 0;

                SELECT 
                    `3DMANAGER_C_STATE_PRINT_ID` AS ID,
                    `3DMANAGER_C_STATE_PRINT_NAME` AS DESCRIPTION
                FROM `3DMANAGER_C_STATE_PRINT`;
            END;
            """;

            await DatabaseSeederhelper.ExecuteAsync(_connectionString, sql);
        }
    }
}
