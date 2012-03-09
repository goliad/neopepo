--Madres Repetidas y Bebes con reingreso en neo
select * FROM Madres
WHERE dni='38599864'

--modificacion
select * from Bebes
where madreID = 29

Update Bebes set madreID = 346
where madreID = 347

select * from HistorialNeos 
where bebeID = 29


Update HistorialNeos 
set tipo = 'Reingreso',
	bebeID = 29
where bebeID=44

select * from Agenda
where bebeID = 274


--Departamentos sin codigo o mal asignados
Update Madres set departamentoID = 40
where dni = '34396302'

select * from Madres
where departamentoID=1


--Agregar Historial a la Agendaselect * from HistorialAgendas
where bebeID=120

select * from Madres
where dni='37734563'

select * from Bebes
where madreID=386

INSERT INTO HistorialAgendas (bebeID,descripcion,fecha) 
VALUES(386,
'RN Gerez Ximena  asistida en el consultorio 10 Dra. Ana Real se encuentra bien.-	Preguntar por vacuna BCG',
GETDATE())

update HistorialAgendas set bebeID = 175
where descripcion ='RN - Hugo Enrique Bravo asistido en la upa del gral paz. Paciente en buen estado.'