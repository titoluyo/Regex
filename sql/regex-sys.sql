select * from sys.dm_clr_properties

select * from sys.dm_clr_appdomains
select * from sys.dm_clr_loaded_assemblies
select * from sys.dm_clr_tasks

SELECT 
 af.name,
 af.content 
 ,*
FROM sys.assemblies a
INNER JOIN sys.assembly_files af ON a.assembly_id = af.assembly_id 
--WHERE 
-- a.name = 'Microsoft.SqlServer.Types'

