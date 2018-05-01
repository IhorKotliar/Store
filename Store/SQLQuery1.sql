--update JOR_SALE set Action_Id=(SELECT TOP 1 Id from DIC_ACTIONS ORDER BY NEWID()) where Action_Id is null;
SELECT *FROM JOR_SALE WHERE Action_Id=1;