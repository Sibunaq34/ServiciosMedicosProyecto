/* =========================================================
   Persona C - Kenneth
   Procedimientos almacenados para:
   OFE1 Oferentes, OFE2 Concursos, OFE3 Preparacion academica,
   GEN5 Instituciones educativas.

   Requisito JSON:
   - sp_oferentes_crear y sp_oferentes_actualizar reciben arreglos JSON
     en texto para correos, telefonos y concursos.
   - Se usan JSON_VALID, JSON_LENGTH y JSON_EXTRACT para evitar depender
     de JSON_TABLE.
   ========================================================= */

USE ServiciosMedicos;

DELIMITER $$

/* =========================================================
   Persona C - Kenneth - GEN5 Instituciones educativas
   ========================================================= */

DROP PROCEDURE IF EXISTS sp_instituciones_listar$$
CREATE PROCEDURE sp_instituciones_listar(
    IN pPagina INT,
    IN pTamanoPagina INT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vPagina INT DEFAULT 1;
    DECLARE vTamanoPagina INT DEFAULT 10;
    DECLARE vOffset INT DEFAULT 0;

    SET vPagina = IFNULL(NULLIF(pPagina, 0), 1);
    SET vTamanoPagina = IFNULL(NULLIF(pTamanoPagina, 0), 10);

    IF vPagina < 1 THEN
        SET vPagina = 1;
    END IF;

    IF vTamanoPagina < 1 THEN
        SET vTamanoPagina = 10;
    END IF;

    SET vOffset = (vPagina - 1) * vTamanoPagina;

    SELECT
        id_insti_edu AS IdInstitucion,
        codigo_insti AS Codigo,
        nombre AS Nombre
    FROM Institu_educa
    ORDER BY nombre ASC
    LIMIT vTamanoPagina OFFSET vOffset;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Consultar GEN5',
        JSON_OBJECT(
            'mensaje', 'El usuario consulta instituciones educativas',
            'pagina', vPagina,
            'tamanoPagina', vTamanoPagina
        )
    );
END$$

DROP PROCEDURE IF EXISTS sp_instituciones_obtener$$
CREATE PROCEDURE sp_instituciones_obtener(
    IN pIdInstitucion INT,
    IN pIdUsuario INT
)
BEGIN
    IF pIdInstitucion IS NULL OR pIdInstitucion <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa es requerida.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Institu_educa
        WHERE id_insti_edu = pIdInstitucion
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa no existe.';
    END IF;

    SELECT
        id_insti_edu AS IdInstitucion,
        codigo_insti AS Codigo,
        nombre AS Nombre
    FROM Institu_educa
    WHERE id_insti_edu = pIdInstitucion;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Consultar GEN5',
        JSON_OBJECT(
            'mensaje', 'El usuario consulta una institucion educativa',
            'idInstitucion', pIdInstitucion
        )
    );
END$$

DROP PROCEDURE IF EXISTS sp_instituciones_crear$$
CREATE PROCEDURE sp_instituciones_crear(
    IN pCodigo VARCHAR(30),
    IN pNombre VARCHAR(150),
    IN pIdUsuario INT
)
BEGIN
    DECLARE vIdInstitucion INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pCodigo IS NULL OR TRIM(pCodigo) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El codigo de la institucion es requerido.';
    END IF;

    IF pNombre IS NULL OR TRIM(pNombre) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre de la institucion es requerido.';
    END IF;

    IF CHAR_LENGTH(TRIM(pNombre)) > 150 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre de la institucion no puede superar 150 caracteres.';
    END IF;

    IF NOT REGEXP_LIKE(TRIM(pNombre), '^[[:alpha:] ]+$') THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre solo puede contener letras y espacios.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM Institu_educa
        WHERE codigo_insti = TRIM(pCodigo)
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El codigo de la institucion ya existe.';
    END IF;

    START TRANSACTION;

    INSERT INTO Institu_educa(codigo_insti, nombre)
    VALUES (TRIM(pCodigo), TRIM(pNombre));

    SET vIdInstitucion = LAST_INSERT_ID();

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Crear GEN5',
        JSON_OBJECT(
            'mensaje', 'Se crea institucion educativa',
            'registro', JSON_OBJECT(
                'idInstitucion', vIdInstitucion,
                'codigo', TRIM(pCodigo),
                'nombre', TRIM(pNombre)
            )
        )
    );

    COMMIT;

    SELECT vIdInstitucion AS IdInstitucion;
END$$

DROP PROCEDURE IF EXISTS sp_instituciones_actualizar$$
CREATE PROCEDURE sp_instituciones_actualizar(
    IN pIdInstitucion INT,
    IN pCodigo VARCHAR(30),
    IN pNombre VARCHAR(150),
    IN pIdUsuario INT
)
BEGIN
    DECLARE vAnterior LONGTEXT;
    DECLARE vActual LONGTEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdInstitucion IS NULL OR pIdInstitucion <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa es requerida.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Institu_educa
        WHERE id_insti_edu = pIdInstitucion
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa no existe.';
    END IF;

    IF pCodigo IS NULL OR TRIM(pCodigo) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El codigo de la institucion es requerido.';
    END IF;

    IF pNombre IS NULL OR TRIM(pNombre) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre de la institucion es requerido.';
    END IF;

    IF CHAR_LENGTH(TRIM(pNombre)) > 150 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre de la institucion no puede superar 150 caracteres.';
    END IF;

    IF NOT REGEXP_LIKE(TRIM(pNombre), '^[[:alpha:] ]+$') THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre solo puede contener letras y espacios.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM Institu_educa
        WHERE codigo_insti = TRIM(pCodigo)
          AND id_insti_edu <> pIdInstitucion
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El codigo de la institucion ya existe.';
    END IF;

    SELECT JSON_OBJECT(
        'idInstitucion', id_insti_edu,
        'codigo', codigo_insti,
        'nombre', nombre
    )
    INTO vAnterior
    FROM Institu_educa
    WHERE id_insti_edu = pIdInstitucion;

    START TRANSACTION;

    UPDATE Institu_educa
    SET
        codigo_insti = TRIM(pCodigo),
        nombre = TRIM(pNombre)
    WHERE id_insti_edu = pIdInstitucion;

    SELECT JSON_OBJECT(
        'idInstitucion', id_insti_edu,
        'codigo', codigo_insti,
        'nombre', nombre
    )
    INTO vActual
    FROM Institu_educa
    WHERE id_insti_edu = pIdInstitucion;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Actualizar GEN5',
        JSON_OBJECT(
            'mensaje', 'Se actualiza institucion educativa',
            'anterior', JSON_EXTRACT(vAnterior, '$'),
            'actual', JSON_EXTRACT(vActual, '$')
        )
    );

    COMMIT;
END$$

DROP PROCEDURE IF EXISTS sp_instituciones_eliminar$$
CREATE PROCEDURE sp_instituciones_eliminar(
    IN pIdInstitucion INT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vAnterior LONGTEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdInstitucion IS NULL OR pIdInstitucion <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa es requerida.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Institu_educa
        WHERE id_insti_edu = pIdInstitucion
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa no existe.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM Prepara_academica
        WHERE id_insti_edu = pIdInstitucion
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'No se puede eliminar un registro con datos relacionados.';
    END IF;

    SELECT JSON_OBJECT(
        'idInstitucion', id_insti_edu,
        'codigo', codigo_insti,
        'nombre', nombre
    )
    INTO vAnterior
    FROM Institu_educa
    WHERE id_insti_edu = pIdInstitucion;

    START TRANSACTION;

    DELETE FROM Institu_educa
    WHERE id_insti_edu = pIdInstitucion;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Eliminar GEN5',
        JSON_OBJECT(
            'mensaje', 'Se elimina institucion educativa',
            'registroEliminado', JSON_EXTRACT(vAnterior, '$')
        )
    );

    COMMIT;
END$$

/* =========================================================
   Persona C - Kenneth - OFE2 Concursos
   ========================================================= */

DROP PROCEDURE IF EXISTS sp_concursos_listar$$
CREATE PROCEDURE sp_concursos_listar(
    IN pPagina INT,
    IN pTamanoPagina INT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vPagina INT DEFAULT 1;
    DECLARE vTamanoPagina INT DEFAULT 10;
    DECLARE vOffset INT DEFAULT 0;

    SET vPagina = IFNULL(NULLIF(pPagina, 0), 1);
    SET vTamanoPagina = IFNULL(NULLIF(pTamanoPagina, 0), 10);

    IF vPagina < 1 THEN
        SET vPagina = 1;
    END IF;

    IF vTamanoPagina < 1 THEN
        SET vTamanoPagina = 10;
    END IF;

    SET vOffset = (vPagina - 1) * vTamanoPagina;

    SELECT
        id_concursos AS IdConcurso,
        codigo_concurso AS Codigo,
        nombre_concurso AS Nombre,
        fecha_inicio AS FechaInicio,
        fecha_fin AS FechaFin,
        estado_concur AS Estado
    FROM Concursos
    ORDER BY fecha_inicio DESC, nombre_concurso ASC
    LIMIT vTamanoPagina OFFSET vOffset;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Consultar OFE2',
        JSON_OBJECT(
            'mensaje', 'El usuario consulta concursos',
            'pagina', vPagina,
            'tamanoPagina', vTamanoPagina
        )
    );
END$$

DROP PROCEDURE IF EXISTS sp_concursos_obtener$$
CREATE PROCEDURE sp_concursos_obtener(
    IN pIdConcurso INT,
    IN pIdUsuario INT
)
BEGIN
    IF pIdConcurso IS NULL OR pIdConcurso <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El concurso es requerido.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Concursos
        WHERE id_concursos = pIdConcurso
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El concurso no existe.';
    END IF;

    SELECT
        id_concursos AS IdConcurso,
        codigo_concurso AS Codigo,
        nombre_concurso AS Nombre,
        fecha_inicio AS FechaInicio,
        fecha_fin AS FechaFin,
        estado_concur AS Estado
    FROM Concursos
    WHERE id_concursos = pIdConcurso;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Consultar OFE2',
        JSON_OBJECT(
            'mensaje', 'El usuario consulta un concurso',
            'idConcurso', pIdConcurso
        )
    );
END$$

DROP PROCEDURE IF EXISTS sp_concursos_crear$$
CREATE PROCEDURE sp_concursos_crear(
    IN pCodigo VARCHAR(30),
    IN pNombre VARCHAR(150),
    IN pFechaInicio DATE,
    IN pFechaFin DATE,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vIdConcurso INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pCodigo IS NULL OR TRIM(pCodigo) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El codigo del concurso es requerido.';
    END IF;

    IF pNombre IS NULL OR TRIM(pNombre) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre del concurso es requerido.';
    END IF;

    IF pFechaInicio IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La fecha de inicio del concurso es requerida.';
    END IF;

    IF pFechaFin IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La fecha de fin del concurso es requerida.';
    END IF;

    IF pFechaFin < pFechaInicio THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La fecha de fin debe ser mayor o igual a la fecha de inicio.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM Concursos
        WHERE codigo_concurso = TRIM(pCodigo)
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El codigo del concurso ya existe.';
    END IF;

    START TRANSACTION;

    INSERT INTO Concursos(
        codigo_concurso,
        nombre_concurso,
        fecha_inicio,
        fecha_fin,
        estado_concur
    )
    VALUES (
        TRIM(pCodigo),
        TRIM(pNombre),
        pFechaInicio,
        pFechaFin,
        'Vigente'
    );

    SET vIdConcurso = LAST_INSERT_ID();

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Crear OFE2',
        JSON_OBJECT(
            'mensaje', 'Se crea concurso',
            'registro', JSON_OBJECT(
                'idConcurso', vIdConcurso,
                'codigo', TRIM(pCodigo),
                'nombre', TRIM(pNombre),
                'fechaInicio', pFechaInicio,
                'fechaFin', pFechaFin,
                'estado', 'Vigente'
            )
        )
    );

    COMMIT;

    SELECT vIdConcurso AS IdConcurso;
END$$

DROP PROCEDURE IF EXISTS sp_concursos_actualizar$$
CREATE PROCEDURE sp_concursos_actualizar(
    IN pIdConcurso INT,
    IN pCodigo VARCHAR(30),
    IN pNombre VARCHAR(150),
    IN pFechaInicio DATE,
    IN pFechaFin DATE,
    IN pEstado VARCHAR(20),
    IN pIdUsuario INT
)
BEGIN
    DECLARE vAnterior LONGTEXT;
    DECLARE vActual LONGTEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdConcurso IS NULL OR pIdConcurso <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El concurso es requerido.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Concursos
        WHERE id_concursos = pIdConcurso
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El concurso no existe.';
    END IF;

    IF pCodigo IS NULL OR TRIM(pCodigo) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El codigo del concurso es requerido.';
    END IF;

    IF pNombre IS NULL OR TRIM(pNombre) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre del concurso es requerido.';
    END IF;

    IF pFechaInicio IS NULL OR pFechaFin IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Las fechas del concurso son requeridas.';
    END IF;

    IF pFechaFin < pFechaInicio THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La fecha de fin debe ser mayor o igual a la fecha de inicio.';
    END IF;

    IF pEstado IS NULL OR pEstado NOT IN ('Vigente', 'Vencido') THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El estado del concurso debe ser Vigente o Vencido.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM Concursos
        WHERE codigo_concurso = TRIM(pCodigo)
          AND id_concursos <> pIdConcurso
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El codigo del concurso ya existe.';
    END IF;

    SELECT JSON_OBJECT(
        'idConcurso', id_concursos,
        'codigo', codigo_concurso,
        'nombre', nombre_concurso,
        'fechaInicio', fecha_inicio,
        'fechaFin', fecha_fin,
        'estado', estado_concur
    )
    INTO vAnterior
    FROM Concursos
    WHERE id_concursos = pIdConcurso;

    START TRANSACTION;

    UPDATE Concursos
    SET
        codigo_concurso = TRIM(pCodigo),
        nombre_concurso = TRIM(pNombre),
        fecha_inicio = pFechaInicio,
        fecha_fin = pFechaFin,
        estado_concur = pEstado
    WHERE id_concursos = pIdConcurso;

    SELECT JSON_OBJECT(
        'idConcurso', id_concursos,
        'codigo', codigo_concurso,
        'nombre', nombre_concurso,
        'fechaInicio', fecha_inicio,
        'fechaFin', fecha_fin,
        'estado', estado_concur
    )
    INTO vActual
    FROM Concursos
    WHERE id_concursos = pIdConcurso;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Actualizar OFE2',
        JSON_OBJECT(
            'mensaje', 'Se actualiza concurso',
            'anterior', JSON_EXTRACT(vAnterior, '$'),
            'actual', JSON_EXTRACT(vActual, '$')
        )
    );

    COMMIT;
END$$

DROP PROCEDURE IF EXISTS sp_concursos_cambiar_estado$$
CREATE PROCEDURE sp_concursos_cambiar_estado(
    IN pIdConcurso INT,
    IN pEstado VARCHAR(20),
    IN pIdUsuario INT
)
BEGIN
    DECLARE vEstadoAnterior VARCHAR(20);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdConcurso IS NULL OR pIdConcurso <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El concurso es requerido.';
    END IF;

    IF pEstado IS NULL OR pEstado NOT IN ('Vigente', 'Vencido') THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El estado del concurso debe ser Vigente o Vencido.';
    END IF;

    SELECT estado_concur
    INTO vEstadoAnterior
    FROM Concursos
    WHERE id_concursos = pIdConcurso;

    IF vEstadoAnterior IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El concurso no existe.';
    END IF;

    START TRANSACTION;

    UPDATE Concursos
    SET estado_concur = pEstado
    WHERE id_concursos = pIdConcurso;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Cambiar estado OFE2',
        JSON_OBJECT(
            'mensaje', 'Se cambia estado de concurso',
            'idConcurso', pIdConcurso,
            'estadoAnterior', vEstadoAnterior,
            'estadoActual', pEstado
        )
    );

    COMMIT;
END$$

DROP PROCEDURE IF EXISTS sp_concursos_eliminar$$
CREATE PROCEDURE sp_concursos_eliminar(
    IN pIdConcurso INT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vAnterior LONGTEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdConcurso IS NULL OR pIdConcurso <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El concurso es requerido.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Concursos
        WHERE id_concursos = pIdConcurso
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El concurso no existe.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM Oferente_concur
        WHERE id_concursos = pIdConcurso
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'No se puede eliminar un registro con datos relacionados.';
    END IF;

    SELECT JSON_OBJECT(
        'idConcurso', id_concursos,
        'codigo', codigo_concurso,
        'nombre', nombre_concurso,
        'fechaInicio', fecha_inicio,
        'fechaFin', fecha_fin,
        'estado', estado_concur
    )
    INTO vAnterior
    FROM Concursos
    WHERE id_concursos = pIdConcurso;

    START TRANSACTION;

    DELETE FROM Concursos
    WHERE id_concursos = pIdConcurso;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Eliminar OFE2',
        JSON_OBJECT(
            'mensaje', 'Se elimina concurso',
            'registroEliminado', JSON_EXTRACT(vAnterior, '$')
        )
    );

    COMMIT;
END$$

/* =========================================================
   Persona C - Kenneth - OFE1 Oferentes
   ========================================================= */

DROP PROCEDURE IF EXISTS sp_oferentes_listar$$
CREATE PROCEDURE sp_oferentes_listar(
    IN pPagina INT,
    IN pTamanoPagina INT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vPagina INT DEFAULT 1;
    DECLARE vTamanoPagina INT DEFAULT 10;
    DECLARE vOffset INT DEFAULT 0;

    SET vPagina = IFNULL(NULLIF(pPagina, 0), 1);
    SET vTamanoPagina = IFNULL(NULLIF(pTamanoPagina, 0), 10);

    IF vPagina < 1 THEN
        SET vPagina = 1;
    END IF;

    IF vTamanoPagina < 1 THEN
        SET vTamanoPagina = 10;
    END IF;

    SET vOffset = (vPagina - 1) * vTamanoPagina;

    SELECT
        o.id_oferente AS IdOferente,
        p.id_persona AS IdPersona,
        p.identificacion AS Identificacion,
        p.tipo_identificacion AS TipoIdentificacion,
        p.nombre_comple AS NombreCompleto,
        p.fecha_naci AS FechaNacimiento,
        o.fecha_regis AS FechaRegistro,
        COALESCE((
            SELECT JSON_ARRAYAGG(oc.correo)
            FROM Oferente_correo oc
            WHERE oc.id_oferente = o.id_oferente
        ), JSON_ARRAY()) AS Correos,
        COALESCE((
            SELECT JSON_ARRAYAGG(ot.telefono)
            FROM Oferente_telf ot
            WHERE ot.id_oferente = o.id_oferente
        ), JSON_ARRAY()) AS Telefonos,
        COALESCE((
            SELECT JSON_ARRAYAGG(
                JSON_OBJECT(
                    'idConcurso', c.id_concursos,
                    'codigo', c.codigo_concurso,
                    'nombre', c.nombre_concurso,
                    'estado', c.estado_concur
                )
            )
            FROM Oferente_concur ofc
            INNER JOIN Concursos c ON c.id_concursos = ofc.id_concursos
            WHERE ofc.id_oferente = o.id_oferente
        ), JSON_ARRAY()) AS Concursos
    FROM Oferentes o
    INNER JOIN Personas p ON p.id_persona = o.id_persona
    ORDER BY o.fecha_regis DESC, p.nombre_comple ASC
    LIMIT vTamanoPagina OFFSET vOffset;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Consultar OFE1',
        JSON_OBJECT(
            'mensaje', 'El usuario consulta oferentes',
            'pagina', vPagina,
            'tamanoPagina', vTamanoPagina
        )
    );
END$$

DROP PROCEDURE IF EXISTS sp_oferentes_obtener$$
CREATE PROCEDURE sp_oferentes_obtener(
    IN pIdOferente INT,
    IN pIdUsuario INT
)
BEGIN
    IF pIdOferente IS NULL OR pIdOferente <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente es requerido.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Oferentes
        WHERE id_oferente = pIdOferente
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente no existe.';
    END IF;

    SELECT
        o.id_oferente AS IdOferente,
        p.id_persona AS IdPersona,
        p.identificacion AS Identificacion,
        p.tipo_identificacion AS TipoIdentificacion,
        p.nombre_comple AS NombreCompleto,
        p.fecha_naci AS FechaNacimiento,
        o.fecha_regis AS FechaRegistro,
        COALESCE((
            SELECT JSON_ARRAYAGG(oc.correo)
            FROM Oferente_correo oc
            WHERE oc.id_oferente = o.id_oferente
        ), JSON_ARRAY()) AS Correos,
        COALESCE((
            SELECT JSON_ARRAYAGG(ot.telefono)
            FROM Oferente_telf ot
            WHERE ot.id_oferente = o.id_oferente
        ), JSON_ARRAY()) AS Telefonos,
        COALESCE((
            SELECT JSON_ARRAYAGG(
                JSON_OBJECT(
                    'idConcurso', c.id_concursos,
                    'codigo', c.codigo_concurso,
                    'nombre', c.nombre_concurso,
                    'estado', c.estado_concur
                )
            )
            FROM Oferente_concur ofc
            INNER JOIN Concursos c ON c.id_concursos = ofc.id_concursos
            WHERE ofc.id_oferente = o.id_oferente
        ), JSON_ARRAY()) AS Concursos
    FROM Oferentes o
    INNER JOIN Personas p ON p.id_persona = o.id_persona
    WHERE o.id_oferente = pIdOferente;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Consultar OFE1',
        JSON_OBJECT(
            'mensaje', 'El usuario consulta un oferente',
            'idOferente', pIdOferente
        )
    );
END$$

DROP PROCEDURE IF EXISTS sp_oferentes_crear$$
CREATE PROCEDURE sp_oferentes_crear(
    IN pIdentificacion VARCHAR(30),
    IN pTipoIdentificacion VARCHAR(20),
    IN pNombreCompleto VARCHAR(150),
    IN pFechaNacimiento DATE,
    IN pCorreosJson LONGTEXT,
    IN pTelefonosJson LONGTEXT,
    IN pConcursosJson LONGTEXT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vIdPersona INT;
    DECLARE vIdOferente INT;
    DECLARE vIndex INT DEFAULT 0;
    DECLARE vTotal INT DEFAULT 0;
    DECLARE vCorreo VARCHAR(150);
    DECLARE vTelefono VARCHAR(20);
    DECLARE vIdConcursoItem INT;
    DECLARE vRegistro LONGTEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdentificacion IS NULL OR TRIM(pIdentificacion) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La identificacion es requerida.';
    END IF;

    IF pTipoIdentificacion IS NULL
       OR pTipoIdentificacion NOT IN ('CedulaIdentidad', 'DIMEX', 'Pasaporte') THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El tipo de identificacion no es valido.';
    END IF;

    IF pNombreCompleto IS NULL OR TRIM(pNombreCompleto) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre completo es requerido.';
    END IF;

    IF pFechaNacimiento IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La fecha de nacimiento es requerida.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM Personas
        WHERE identificacion = TRIM(pIdentificacion)
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El numero de identificacion ya existe.';
    END IF;

    IF pCorreosJson IS NULL OR JSON_VALID(pCorreosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La lista de correos debe ser JSON valido.';
    END IF;

    IF JSON_TYPE(JSON_EXTRACT(pCorreosJson, '$')) <> 'ARRAY'
       OR JSON_LENGTH(pCorreosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Debe indicar al menos un correo.';
    END IF;

    IF pTelefonosJson IS NULL OR JSON_VALID(pTelefonosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La lista de telefonos debe ser JSON valido.';
    END IF;

    IF JSON_TYPE(JSON_EXTRACT(pTelefonosJson, '$')) <> 'ARRAY'
       OR JSON_LENGTH(pTelefonosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Debe indicar al menos un telefono.';
    END IF;

    IF pConcursosJson IS NULL OR JSON_VALID(pConcursosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La lista de concursos debe ser JSON valido.';
    END IF;

    IF JSON_TYPE(JSON_EXTRACT(pConcursosJson, '$')) <> 'ARRAY'
       OR JSON_LENGTH(pConcursosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Debe indicar al menos un concurso.';
    END IF;

    START TRANSACTION;

    INSERT INTO Personas(
        identificacion,
        tipo_identificacion,
        nombre_comple,
        fecha_naci,
        tipo_perso
    )
    VALUES (
        TRIM(pIdentificacion),
        pTipoIdentificacion,
        TRIM(pNombreCompleto),
        pFechaNacimiento,
        'Oferente'
    );

    SET vIdPersona = LAST_INSERT_ID();

    INSERT INTO Oferentes(id_persona)
    VALUES (vIdPersona);

    SET vIdOferente = LAST_INSERT_ID();

    SET vIndex = 0;
    SET vTotal = JSON_LENGTH(pCorreosJson);
    WHILE vIndex < vTotal DO
        SET vCorreo = TRIM(JSON_UNQUOTE(JSON_EXTRACT(pCorreosJson, CONCAT('$[', vIndex, ']'))));

        IF vCorreo IS NULL OR vCorreo = '' THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'El correo es requerido.';
        END IF;

        IF vCorreo NOT REGEXP '^[^@[:space:]]+@[^@[:space:]]+\\.[^@[:space:]]+$' THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'El correo no tiene un formato valido.';
        END IF;

        INSERT INTO Oferente_correo(id_oferente, correo)
        VALUES (vIdOferente, vCorreo);

        SET vIndex = vIndex + 1;
    END WHILE;

    SET vIndex = 0;
    SET vTotal = JSON_LENGTH(pTelefonosJson);
    WHILE vIndex < vTotal DO
        SET vTelefono = TRIM(JSON_UNQUOTE(JSON_EXTRACT(pTelefonosJson, CONCAT('$[', vIndex, ']'))));

        IF vTelefono IS NULL OR vTelefono = '' THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'El telefono es requerido.';
        END IF;

        INSERT INTO Oferente_telf(id_oferente, telefono)
        VALUES (vIdOferente, vTelefono);

        SET vIndex = vIndex + 1;
    END WHILE;

    SET vIndex = 0;
    SET vTotal = JSON_LENGTH(pConcursosJson);
    WHILE vIndex < vTotal DO
        SET vIdConcursoItem = CAST(JSON_UNQUOTE(JSON_EXTRACT(pConcursosJson, CONCAT('$[', vIndex, ']'))) AS UNSIGNED);

        IF vIdConcursoItem IS NULL OR vIdConcursoItem <= 0 THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'El concurso indicado no es valido.';
        END IF;

        IF NOT EXISTS (
            SELECT 1
            FROM Concursos
            WHERE id_concursos = vIdConcursoItem
        ) THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Uno de los concursos indicados no existe.';
        END IF;

        IF EXISTS (
            SELECT 1
            FROM Oferente_concur
            WHERE id_oferente = vIdOferente
              AND id_concursos = vIdConcursoItem
        ) THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'No se deben repetir concursos para el oferente.';
        END IF;

        INSERT INTO Oferente_concur(id_oferente, id_concursos)
        VALUES (vIdOferente, vIdConcursoItem);

        SET vIndex = vIndex + 1;
    END WHILE;

    SET vRegistro = JSON_OBJECT(
        'idOferente', vIdOferente,
        'idPersona', vIdPersona,
        'identificacion', TRIM(pIdentificacion),
        'tipoIdentificacion', pTipoIdentificacion,
        'nombreCompleto', TRIM(pNombreCompleto),
        'fechaNacimiento', pFechaNacimiento,
        'correos', JSON_EXTRACT(pCorreosJson, '$'),
        'telefonos', JSON_EXTRACT(pTelefonosJson, '$'),
        'concursos', JSON_EXTRACT(pConcursosJson, '$')
    );

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Crear OFE1',
        JSON_OBJECT(
            'mensaje', 'Se crea oferente',
            'registro', JSON_EXTRACT(vRegistro, '$')
        )
    );

    COMMIT;

    SELECT vIdOferente AS IdOferente;
END$$

DROP PROCEDURE IF EXISTS sp_oferentes_actualizar$$
CREATE PROCEDURE sp_oferentes_actualizar(
    IN pIdOferente INT,
    IN pIdentificacion VARCHAR(30),
    IN pTipoIdentificacion VARCHAR(20),
    IN pNombreCompleto VARCHAR(150),
    IN pFechaNacimiento DATE,
    IN pCorreosJson LONGTEXT,
    IN pTelefonosJson LONGTEXT,
    IN pConcursosJson LONGTEXT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vIdPersona INT;
    DECLARE vIndex INT DEFAULT 0;
    DECLARE vTotal INT DEFAULT 0;
    DECLARE vCorreo VARCHAR(150);
    DECLARE vTelefono VARCHAR(20);
    DECLARE vIdConcursoItem INT;
    DECLARE vAnterior LONGTEXT;
    DECLARE vActual LONGTEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdOferente IS NULL OR pIdOferente <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente es requerido.';
    END IF;

    SELECT id_persona
    INTO vIdPersona
    FROM Oferentes
    WHERE id_oferente = pIdOferente;

    IF vIdPersona IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente no existe.';
    END IF;

    IF pIdentificacion IS NULL OR TRIM(pIdentificacion) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La identificacion es requerida.';
    END IF;

    IF pTipoIdentificacion IS NULL
       OR pTipoIdentificacion NOT IN ('CedulaIdentidad', 'DIMEX', 'Pasaporte') THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El tipo de identificacion no es valido.';
    END IF;

    IF pNombreCompleto IS NULL OR TRIM(pNombreCompleto) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El nombre completo es requerido.';
    END IF;

    IF pFechaNacimiento IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La fecha de nacimiento es requerida.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM Personas
        WHERE identificacion = TRIM(pIdentificacion)
          AND id_persona <> vIdPersona
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El numero de identificacion ya existe.';
    END IF;

    IF pCorreosJson IS NULL OR JSON_VALID(pCorreosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La lista de correos debe ser JSON valido.';
    END IF;

    IF JSON_TYPE(JSON_EXTRACT(pCorreosJson, '$')) <> 'ARRAY'
       OR JSON_LENGTH(pCorreosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Debe indicar al menos un correo.';
    END IF;

    IF pTelefonosJson IS NULL OR JSON_VALID(pTelefonosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La lista de telefonos debe ser JSON valido.';
    END IF;

    IF JSON_TYPE(JSON_EXTRACT(pTelefonosJson, '$')) <> 'ARRAY'
       OR JSON_LENGTH(pTelefonosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Debe indicar al menos un telefono.';
    END IF;

    IF pConcursosJson IS NULL OR JSON_VALID(pConcursosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La lista de concursos debe ser JSON valido.';
    END IF;

    IF JSON_TYPE(JSON_EXTRACT(pConcursosJson, '$')) <> 'ARRAY'
       OR JSON_LENGTH(pConcursosJson) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Debe indicar al menos un concurso.';
    END IF;

    SELECT JSON_OBJECT(
        'idOferente', o.id_oferente,
        'idPersona', p.id_persona,
        'identificacion', p.identificacion,
        'tipoIdentificacion', p.tipo_identificacion,
        'nombreCompleto', p.nombre_comple,
        'fechaNacimiento', p.fecha_naci,
        'correos', COALESCE((
            SELECT JSON_ARRAYAGG(oc.correo)
            FROM Oferente_correo oc
            WHERE oc.id_oferente = o.id_oferente
        ), JSON_ARRAY()),
        'telefonos', COALESCE((
            SELECT JSON_ARRAYAGG(ot.telefono)
            FROM Oferente_telf ot
            WHERE ot.id_oferente = o.id_oferente
        ), JSON_ARRAY()),
        'concursos', COALESCE((
            SELECT JSON_ARRAYAGG(ofc.id_concursos)
            FROM Oferente_concur ofc
            WHERE ofc.id_oferente = o.id_oferente
        ), JSON_ARRAY())
    )
    INTO vAnterior
    FROM Oferentes o
    INNER JOIN Personas p ON p.id_persona = o.id_persona
    WHERE o.id_oferente = pIdOferente;

    START TRANSACTION;

    UPDATE Personas
    SET
        identificacion = TRIM(pIdentificacion),
        tipo_identificacion = pTipoIdentificacion,
        nombre_comple = TRIM(pNombreCompleto),
        fecha_naci = pFechaNacimiento,
        tipo_perso = 'Oferente'
    WHERE id_persona = vIdPersona;

    DELETE FROM Oferente_correo
    WHERE id_oferente = pIdOferente;

    DELETE FROM Oferente_telf
    WHERE id_oferente = pIdOferente;

    DELETE FROM Oferente_concur
    WHERE id_oferente = pIdOferente;

    SET vIndex = 0;
    SET vTotal = JSON_LENGTH(pCorreosJson);
    WHILE vIndex < vTotal DO
        SET vCorreo = TRIM(JSON_UNQUOTE(JSON_EXTRACT(pCorreosJson, CONCAT('$[', vIndex, ']'))));

        IF vCorreo IS NULL OR vCorreo = '' THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'El correo es requerido.';
        END IF;

        IF vCorreo NOT REGEXP '^[^@[:space:]]+@[^@[:space:]]+\\.[^@[:space:]]+$' THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'El correo no tiene un formato valido.';
        END IF;

        INSERT INTO Oferente_correo(id_oferente, correo)
        VALUES (pIdOferente, vCorreo);

        SET vIndex = vIndex + 1;
    END WHILE;

    SET vIndex = 0;
    SET vTotal = JSON_LENGTH(pTelefonosJson);
    WHILE vIndex < vTotal DO
        SET vTelefono = TRIM(JSON_UNQUOTE(JSON_EXTRACT(pTelefonosJson, CONCAT('$[', vIndex, ']'))));

        IF vTelefono IS NULL OR vTelefono = '' THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'El telefono es requerido.';
        END IF;

        INSERT INTO Oferente_telf(id_oferente, telefono)
        VALUES (pIdOferente, vTelefono);

        SET vIndex = vIndex + 1;
    END WHILE;

    SET vIndex = 0;
    SET vTotal = JSON_LENGTH(pConcursosJson);
    WHILE vIndex < vTotal DO
        SET vIdConcursoItem = CAST(JSON_UNQUOTE(JSON_EXTRACT(pConcursosJson, CONCAT('$[', vIndex, ']'))) AS UNSIGNED);

        IF vIdConcursoItem IS NULL OR vIdConcursoItem <= 0 THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'El concurso indicado no es valido.';
        END IF;

        IF NOT EXISTS (
            SELECT 1
            FROM Concursos
            WHERE id_concursos = vIdConcursoItem
        ) THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'Uno de los concursos indicados no existe.';
        END IF;

        IF EXISTS (
            SELECT 1
            FROM Oferente_concur
            WHERE id_oferente = pIdOferente
              AND id_concursos = vIdConcursoItem
        ) THEN
            SIGNAL SQLSTATE '45000'
                SET MESSAGE_TEXT = 'No se deben repetir concursos para el oferente.';
        END IF;

        INSERT INTO Oferente_concur(id_oferente, id_concursos)
        VALUES (pIdOferente, vIdConcursoItem);

        SET vIndex = vIndex + 1;
    END WHILE;

    SET vActual = JSON_OBJECT(
        'idOferente', pIdOferente,
        'idPersona', vIdPersona,
        'identificacion', TRIM(pIdentificacion),
        'tipoIdentificacion', pTipoIdentificacion,
        'nombreCompleto', TRIM(pNombreCompleto),
        'fechaNacimiento', pFechaNacimiento,
        'correos', JSON_EXTRACT(pCorreosJson, '$'),
        'telefonos', JSON_EXTRACT(pTelefonosJson, '$'),
        'concursos', JSON_EXTRACT(pConcursosJson, '$')
    );

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Actualizar OFE1',
        JSON_OBJECT(
            'mensaje', 'Se actualiza oferente',
            'anterior', JSON_EXTRACT(vAnterior, '$'),
            'actual', JSON_EXTRACT(vActual, '$')
        )
    );

    COMMIT;
END$$

DROP PROCEDURE IF EXISTS sp_oferentes_eliminar$$
CREATE PROCEDURE sp_oferentes_eliminar(
    IN pIdOferente INT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vIdPersona INT;
    DECLARE vAnterior LONGTEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdOferente IS NULL OR pIdOferente <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente es requerido.';
    END IF;

    SELECT id_persona
    INTO vIdPersona
    FROM Oferentes
    WHERE id_oferente = pIdOferente;

    IF vIdPersona IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente no existe.';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM Prepara_academica
        WHERE id_oferente = pIdOferente
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'No se puede eliminar un registro con datos relacionados.';
    END IF;

    SELECT JSON_OBJECT(
        'idOferente', o.id_oferente,
        'idPersona', p.id_persona,
        'identificacion', p.identificacion,
        'tipoIdentificacion', p.tipo_identificacion,
        'nombreCompleto', p.nombre_comple,
        'fechaNacimiento', p.fecha_naci,
        'correos', COALESCE((
            SELECT JSON_ARRAYAGG(oc.correo)
            FROM Oferente_correo oc
            WHERE oc.id_oferente = o.id_oferente
        ), JSON_ARRAY()),
        'telefonos', COALESCE((
            SELECT JSON_ARRAYAGG(ot.telefono)
            FROM Oferente_telf ot
            WHERE ot.id_oferente = o.id_oferente
        ), JSON_ARRAY()),
        'concursos', COALESCE((
            SELECT JSON_ARRAYAGG(ofc.id_concursos)
            FROM Oferente_concur ofc
            WHERE ofc.id_oferente = o.id_oferente
        ), JSON_ARRAY())
    )
    INTO vAnterior
    FROM Oferentes o
    INNER JOIN Personas p ON p.id_persona = o.id_persona
    WHERE o.id_oferente = pIdOferente;

    START TRANSACTION;

    DELETE FROM Oferente_concur
    WHERE id_oferente = pIdOferente;

    DELETE FROM Oferente_correo
    WHERE id_oferente = pIdOferente;

    DELETE FROM Oferente_telf
    WHERE id_oferente = pIdOferente;

    DELETE FROM Oferentes
    WHERE id_oferente = pIdOferente;

    DELETE FROM Personas
    WHERE id_persona = vIdPersona;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Eliminar OFE1',
        JSON_OBJECT(
            'mensaje', 'Se elimina oferente',
            'registroEliminado', JSON_EXTRACT(vAnterior, '$')
        )
    );

    COMMIT;
END$$

/* =========================================================
   Persona C - Kenneth - OFE3 Preparacion academica
   ========================================================= */

DROP PROCEDURE IF EXISTS sp_preparacion_listar_por_oferente$$
CREATE PROCEDURE sp_preparacion_listar_por_oferente(
    IN pIdOferente INT,
    IN pPagina INT,
    IN pTamanoPagina INT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vPagina INT DEFAULT 1;
    DECLARE vTamanoPagina INT DEFAULT 10;
    DECLARE vOffset INT DEFAULT 0;

    IF pIdOferente IS NULL OR pIdOferente <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente es requerido.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Oferentes
        WHERE id_oferente = pIdOferente
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente no existe.';
    END IF;

    SET vPagina = IFNULL(NULLIF(pPagina, 0), 1);
    SET vTamanoPagina = IFNULL(NULLIF(pTamanoPagina, 0), 10);

    IF vPagina < 1 THEN
        SET vPagina = 1;
    END IF;

    IF vTamanoPagina < 1 THEN
        SET vTamanoPagina = 10;
    END IF;

    SET vOffset = (vPagina - 1) * vTamanoPagina;

    SELECT
        pa.id_pre_academica AS IdPreparacion,
        pa.id_oferente AS IdOferente,
        pa.id_insti_edu AS IdInstitucion,
        ie.codigo_insti AS CodigoInstitucion,
        ie.nombre AS NombreInstitucion,
        pa.titulo_obtenido AS Titulo,
        pa.fecha_inicio AS FechaInicio,
        pa.fecha_fin AS FechaFin
    FROM Prepara_academica pa
    INNER JOIN Institu_educa ie ON ie.id_insti_edu = pa.id_insti_edu
    WHERE pa.id_oferente = pIdOferente
    ORDER BY pa.fecha_inicio DESC, pa.titulo_obtenido ASC
    LIMIT vTamanoPagina OFFSET vOffset;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Consultar OFE3',
        JSON_OBJECT(
            'mensaje', 'El usuario consulta preparacion academica de un oferente',
            'idOferente', pIdOferente,
            'pagina', vPagina,
            'tamanoPagina', vTamanoPagina
        )
    );
END$$

DROP PROCEDURE IF EXISTS sp_preparacion_obtener$$
CREATE PROCEDURE sp_preparacion_obtener(
    IN pIdPreparacion INT,
    IN pIdUsuario INT
)
BEGIN
    IF pIdPreparacion IS NULL OR pIdPreparacion <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La preparacion academica es requerida.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Prepara_academica
        WHERE id_pre_academica = pIdPreparacion
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La preparacion academica no existe.';
    END IF;

    SELECT
        pa.id_pre_academica AS IdPreparacion,
        pa.id_oferente AS IdOferente,
        pa.id_insti_edu AS IdInstitucion,
        ie.codigo_insti AS CodigoInstitucion,
        ie.nombre AS NombreInstitucion,
        pa.titulo_obtenido AS Titulo,
        pa.fecha_inicio AS FechaInicio,
        pa.fecha_fin AS FechaFin
    FROM Prepara_academica pa
    INNER JOIN Institu_educa ie ON ie.id_insti_edu = pa.id_insti_edu
    WHERE pa.id_pre_academica = pIdPreparacion;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Consultar OFE3',
        JSON_OBJECT(
            'mensaje', 'El usuario consulta una preparacion academica',
            'idPreparacion', pIdPreparacion
        )
    );
END$$

DROP PROCEDURE IF EXISTS sp_preparacion_crear$$
CREATE PROCEDURE sp_preparacion_crear(
    IN pIdOferente INT,
    IN pIdInstitucion INT,
    IN pTitulo VARCHAR(100),
    IN pFechaInicio DATE,
    IN pFechaFin DATE,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vIdPreparacion INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdOferente IS NULL OR pIdOferente <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente es requerido.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Oferentes
        WHERE id_oferente = pIdOferente
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El oferente no existe.';
    END IF;

    IF pIdInstitucion IS NULL OR pIdInstitucion <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa es requerida.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Institu_educa
        WHERE id_insti_edu = pIdInstitucion
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa no existe.';
    END IF;

    IF pTitulo IS NULL OR TRIM(pTitulo) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El titulo obtenido es requerido.';
    END IF;

    IF CHAR_LENGTH(TRIM(pTitulo)) > 100 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El titulo obtenido no puede superar 100 caracteres.';
    END IF;

    IF NOT REGEXP_LIKE(TRIM(pTitulo), '^[[:alpha:] ]+$') THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El título obtenido solo puede contener letras y espacios.';
    END IF;

    IF pFechaInicio IS NULL OR pFechaFin IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Las fechas de preparacion academica son requeridas.';
    END IF;

    IF pFechaFin < pFechaInicio THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La fecha de fin debe ser mayor o igual a la fecha de inicio.';
    END IF;

    START TRANSACTION;

    INSERT INTO Prepara_academica(
        id_oferente,
        id_insti_edu,
        titulo_obtenido,
        fecha_inicio,
        fecha_fin
    )
    VALUES (
        pIdOferente,
        pIdInstitucion,
        TRIM(pTitulo),
        pFechaInicio,
        pFechaFin
    );

    SET vIdPreparacion = LAST_INSERT_ID();

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Crear OFE3',
        JSON_OBJECT(
            'mensaje', 'Se crea preparacion academica',
            'registro', JSON_OBJECT(
                'idPreparacion', vIdPreparacion,
                'idOferente', pIdOferente,
                'idInstitucion', pIdInstitucion,
                'titulo', TRIM(pTitulo),
                'fechaInicio', pFechaInicio,
                'fechaFin', pFechaFin
            )
        )
    );

    COMMIT;

    SELECT vIdPreparacion AS IdPreparacion;
END$$

DROP PROCEDURE IF EXISTS sp_preparacion_actualizar$$
CREATE PROCEDURE sp_preparacion_actualizar(
    IN pIdPreparacion INT,
    IN pIdInstitucion INT,
    IN pTitulo VARCHAR(100),
    IN pFechaInicio DATE,
    IN pFechaFin DATE,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vAnterior LONGTEXT;
    DECLARE vActual LONGTEXT;
    DECLARE vIdOferente INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdPreparacion IS NULL OR pIdPreparacion <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La preparacion academica es requerida.';
    END IF;

    SELECT id_oferente
    INTO vIdOferente
    FROM Prepara_academica
    WHERE id_pre_academica = pIdPreparacion;

    IF vIdOferente IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La preparacion academica no existe.';
    END IF;

    IF pIdInstitucion IS NULL OR pIdInstitucion <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa es requerida.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Institu_educa
        WHERE id_insti_edu = pIdInstitucion
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La institucion educativa no existe.';
    END IF;

    IF pTitulo IS NULL OR TRIM(pTitulo) = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El titulo obtenido es requerido.';
    END IF;

    IF CHAR_LENGTH(TRIM(pTitulo)) > 100 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El titulo obtenido no puede superar 100 caracteres.';
    END IF;

    IF NOT REGEXP_LIKE(TRIM(pTitulo), '^[[:alpha:] ]+$') THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El título obtenido solo puede contener letras y espacios.';
    END IF;

    IF pFechaInicio IS NULL OR pFechaFin IS NULL THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Las fechas de preparacion academica son requeridas.';
    END IF;

    IF pFechaFin < pFechaInicio THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La fecha de fin debe ser mayor o igual a la fecha de inicio.';
    END IF;

    SELECT JSON_OBJECT(
        'idPreparacion', id_pre_academica,
        'idOferente', id_oferente,
        'idInstitucion', id_insti_edu,
        'titulo', titulo_obtenido,
        'fechaInicio', fecha_inicio,
        'fechaFin', fecha_fin
    )
    INTO vAnterior
    FROM Prepara_academica
    WHERE id_pre_academica = pIdPreparacion;

    START TRANSACTION;

    UPDATE Prepara_academica
    SET
        id_insti_edu = pIdInstitucion,
        titulo_obtenido = TRIM(pTitulo),
        fecha_inicio = pFechaInicio,
        fecha_fin = pFechaFin
    WHERE id_pre_academica = pIdPreparacion;

    SET vActual = JSON_OBJECT(
        'idPreparacion', pIdPreparacion,
        'idOferente', vIdOferente,
        'idInstitucion', pIdInstitucion,
        'titulo', TRIM(pTitulo),
        'fechaInicio', pFechaInicio,
        'fechaFin', pFechaFin
    );

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Actualizar OFE3',
        JSON_OBJECT(
            'mensaje', 'Se actualiza preparacion academica',
            'anterior', JSON_EXTRACT(vAnterior, '$'),
            'actual', JSON_EXTRACT(vActual, '$')
        )
    );

    COMMIT;
END$$

DROP PROCEDURE IF EXISTS sp_preparacion_eliminar$$
CREATE PROCEDURE sp_preparacion_eliminar(
    IN pIdPreparacion INT,
    IN pIdUsuario INT
)
BEGIN
    DECLARE vAnterior LONGTEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    IF pIdPreparacion IS NULL OR pIdPreparacion <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La preparacion academica es requerida.';
    END IF;

    IF NOT EXISTS (
        SELECT 1
        FROM Prepara_academica
        WHERE id_pre_academica = pIdPreparacion
    ) THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'La preparacion academica no existe.';
    END IF;

    SELECT JSON_OBJECT(
        'idPreparacion', id_pre_academica,
        'idOferente', id_oferente,
        'idInstitucion', id_insti_edu,
        'titulo', titulo_obtenido,
        'fechaInicio', fecha_inicio,
        'fechaFin', fecha_fin
    )
    INTO vAnterior
    FROM Prepara_academica
    WHERE id_pre_academica = pIdPreparacion;

    START TRANSACTION;

    DELETE FROM Prepara_academica
    WHERE id_pre_academica = pIdPreparacion;

    INSERT INTO bitacoras(id_usuario, accion, descripcionAccion)
    VALUES (
        pIdUsuario,
        'Eliminar OFE3',
        JSON_OBJECT(
            'mensaje', 'Se elimina preparacion academica',
            'registroEliminado', JSON_EXTRACT(vAnterior, '$')
        )
    );

    COMMIT;
END$$

DELIMITER ;
