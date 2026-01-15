-- Generic function to notify on any table change
CREATE OR REPLACE FUNCTION notify_db_changes()
RETURNS TRIGGER AS $$
DECLARE
  payload JSON;
  row_id UUID;
BEGIN
  -- Determine the ID of the row (assumes most tables have an "Id" column of type UUID)
  IF (TG_OP = 'DELETE') THEN
    row_id = OLD."Id";
  ELSE
    row_id = NEW."Id";
  END IF;

  payload = json_build_object(
    'table', TG_TABLE_NAME,
    'id', row_id,
    'operation', TG_OP
  );
  
  -- Notify on a generic channel
  PERFORM pg_notify('db_changes', payload::text);
  
  RETURN NULL;
END;
$$ LANGUAGE plpgsql;

-- Helper to drop/recreate trigger on a table
-- Usage: 
-- DROP TRIGGER IF EXISTS tr_notify_users ON "Users";
-- CREATE TRIGGER tr_notify_users AFTER INSERT OR UPDATE OR DELETE ON "Users" FOR EACH ROW EXECUTE FUNCTION notify_db_changes();

-- Apply to Users
DROP TRIGGER IF EXISTS tr_notify_users ON "Users";
CREATE TRIGGER tr_notify_users
AFTER INSERT OR UPDATE OR DELETE
ON "Users"
FOR EACH ROW
EXECUTE FUNCTION notify_db_changes();

-- Apply to Roles (as an example of generic usage)
DROP TRIGGER IF EXISTS tr_notify_roles ON "Roles";
CREATE TRIGGER tr_notify_roles
AFTER INSERT OR UPDATE OR DELETE
ON "Roles"
FOR EACH ROW
EXECUTE FUNCTION notify_db_changes();
