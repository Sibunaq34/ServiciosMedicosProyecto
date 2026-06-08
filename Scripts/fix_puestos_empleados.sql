-- ============================================================
-- Script de correcciones para módulos Puestos y Contratar Empleado
-- Ejecutar en MySQL Workbench sobre la BD ServiciosMedicos
-- ============================================================

USE ServiciosMedicos;

DELIMITER $$

-- ------------------------------------------------------------
-- 1. SP_ListarOferentes
--    Problema: retorna "nombre_comple" sin alias,
--    Dapper no puede mapearlo a OferenteCombo.NombreCompleto
--    Solución: agregar alias "nombre_completo"
-- ------------------------------------------------------------
DROP PROCEDURE IF EXISTS SP_ListarOferentes$$
CREATE PROCEDURE SP_ListarOferentes()
BEGIN
    SELECT
        o.id_oferente,
        p.nombre_comple AS nombre_completo
    FROM Oferentes o
    INNER JOIN Personas p
        ON o.id_persona = p.id_persona
    ORDER BY p.nombre_comple;
END$$

-- ------------------------------------------------------------
-- 2. SP_ListarEmpleados
--    Problema: filtra WHERE estado = 'activo' (minúscula)
--    El ENUM puede tener 'Activo' con mayúscula
--    Solución: quitar el filtro de estado o usar LOWER()
-- ------------------------------------------------------------
DROP PROCEDURE IF EXISTS SP_ListarEmpleados$$
CREATE PROCEDURE SP_ListarEmpleados()
BEGIN
    SELECT
        id_empleado,
        nombre_completo
    FROM Empleados
    ORDER BY nombre_completo;
END$$

-- ------------------------------------------------------------
-- 3. Tabla requitos_puesto
--    Problema: falta la columna nombre_requisito
--    Problema 2: el nombre tiene typo ("requitos" en vez de "requisitos")
--    Solución: agregar columna y renombrar la tabla
-- ------------------------------------------------------------
ALTER TABLE requitos_puesto
    ADD COLUMN nombre_requisito VARCHAR(200) NOT NULL DEFAULT ''
        AFTER id_puesto;

RENAME TABLE requitos_puesto TO Requisitos_Puesto;

-- ------------------------------------------------------------
-- 4. SP_ListarRequisitos
--    Ya apunta a Requisitos_Puesto (nombre correcto tras el rename)
--    Solo se recrea para asegurar consistencia
-- ------------------------------------------------------------
DROP PROCEDURE IF EXISTS SP_ListarRequisitos$$
CREATE PROCEDURE SP_ListarRequisitos(IN pIdPuesto INT)
BEGIN
    SELECT
        id_requisito,
        id_puesto,
        nombre_requisito
    FROM Requisitos_Puesto
    WHERE id_puesto = pIdPuesto
      AND activo = 1;
END$$

-- ------------------------------------------------------------
-- 5. SP_InsertarRequisito — ya apunta a Requisitos_Puesto, ok
-- 6. SP_ActualizarRequisito — ya apunta a Requisitos_Puesto, ok
-- 7. SP_EliminarRequisito — ya apunta a Requisitos_Puesto, ok
-- ------------------------------------------------------------

DELIMITER ;
