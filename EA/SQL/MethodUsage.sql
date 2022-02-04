select o.ea_GUID As CLASSGUID, o.Object_type As CLASSTYPE, 
'Behaviour' As Usage,
o.name As ElementName,
o.Object_Type As ElementType,  
o.stereotype As ElementStereotype, '' As Diagram, pkg.name As PackageName, o.ea_GUID 
from t_operation op,  t_object o INNER JOIN t_package pkg on (o.package_id =pkg.package_id)
where 
     op.ea_GUID = '<Search Term>' AND 
    #DB=SQLSVR#           Substring(op.Behaviour ,1,39)= o.ea_GUID  #DB=SQLSVR#    
    #DB=JET#           op.Behaviour = o.ea_GUID                     #DB=JET# 
    #DB=MYSQL#           op.Behaviour = o.ea_GUID                   #DB=MYSQL#  
    #DB=Other#           op.Behaviour = o.ea_GUID                   #DB=Other#  	
 #DB=ORACLE#        Cast(op.Behaviour As Varchar2(38)) = o.ea_GUID  #DB=ORACLE#
 
 UNION

select op.ea_GUID , 'Operation' , 'Operation Class<--> State', op.name ,  Type, op.stereotype, '', '',op.ea_GUID  
from  t_operation op
where 

    #DB=SQLSVR#     Substring(op.Behaviour ,1,39) = '<Search Term>'              #DB=SQLSVR#
    #DB=JET#     op.Behaviour = '<Search Term>'              #DB=JET#
    #DB=MYSQL#     op.Behaviour = '<Search Term>'              #DB=MYSQL#
	#DB=Other#     op.Behaviour = '<Search Term>'              #DB=Other#
 #DB=ORACLE#   Cast(op.Behaviour As Varchar2(38))  = '<Search Term>'     #DB=ORACLE#
 UNION
select op.ea_GUID , 'Operation' , 'Operation Class<--> State', op.name ,  op.Type, op.stereotype,'', '',op.ea_GUID 
from  t_operation opState, t_operation op 
where 
   opState.ea_GUID = '<Search Term>'  AND
 #DB=SQLSVR#                      Substring(opState.Behaviour,1,39) = op.ea_GUID           #DB=SQLSVR#
#DB=JET#                      opState.Behaviour = op.ea_GUID           #DB=JET#
#DB=MYSQL#                    opState.Behaviour = op.ea_GUID           #DB=MYSQL#
#DB=Other#                    opState.Behaviour = op.ea_GUID           #DB=Other#
#DB=ORACLE#         Cast(op.Behaviour As Varchar2(38)) = op.ea_GUID   #DB=ORACLE#

#DB=ORACLE# 
UNION
select o.ea_GUID , o.Object_Type , 'Call Action', o.name ,  o.Object_Type, o.stereotype ,'', pkg.name, o.ea_GUID
from t_operation op,  t_object o INNER JOIN t_package pkg on (o.package_id =pkg.package_id)
where 
    o.Classifier_GUID = '<Search Term>'
AND  o.Classifier_GUID = op.ea_GUID
/* --------------------------------------------------------Find Call Action -----------------------------------*/

#DB=ORACLE# 


UNION
select o.ea_GUID , o.Object_Type , 'ReturnType', o.name ,  o.Object_Type, o.stereotype,'', pkg.name, o.ea_GUID 
from t_operation op,   t_object o1, t_object o INNER JOIN t_package pkg on (o.package_id =pkg.package_id) 
where 
    op.ea_GUID = '<Search Term>' AND
#DB=SQLSVR#    o.Object_ID = op.Classifier       #DB=SQLSVR# 
#DB=JET#          Format(o.Object_ID) = op.Classifier       #DB=JET#
#DB=MYSQL#          Format(o.Object_ID) = op.Classifier     #DB=MYSQL#
#DB=Other#          o.Object_ID= op.Classifier       #DB=Other#
#DB=ORACLE#     o.Object_ID = op.Classifier          #DB=ORACLE#
AND      op.object_id = o1.object_id

#DB=ORACLE# /* Usage in Sequence Diagram */                 #DB=ORACLE#

UNION
select c.ea_GUID , c.connector_type , 
'Sequence' ,
c.name ,
'Operation2',  
o.stereotype ,d.name, '',c.ea_GUID
from t_connector c, t_object o, t_operation op, t_diagram d, t_diagramlinks dl
where c.end_object_id = o.object_id
 AND o.object_id = op.object_id
 AND '<Search Term>' = op.ea_GUID
 AND dl.diagramID = d.diagram_ID
 AND dl.connectorID = c.connector_id
  AND d.Diagram_Type = "Sequence"

UNION
select
c.ea_GUID , c.connector_type , 
'Sequence' ,
c.name ,
'Operation2',  
o.stereotype, d.name, '', c.ea_GUID

from t_connector c, t_object o1, t_object o, t_operation op, t_diagram d, t_diagramlinks dl
where c.end_object_id = o1.object_id
     AND o1.object_id = o.object_id
 AND o.object_id = op.object_id
 AND '<Search Term>' = op.ea_GUID
 AND dl.diagramID = d.diagram_ID
 AND dl.connectorID = c.connector_id
 AND d.Diagram_Type = "Sequence"
Order By 3,4