-- Function to notify on user changes
CREATE OR REPLACE FUNCTION notify_user_changes()
RETURNS TRIGGER AS $$
DECLARE
  payload JSON;
BEGIN
  payload = json_build_object(
    'id', NEW."Id",
    'operation', TG_OP
  );
  
  PERFORM pg_notify('user_updates', payload::text);
  
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger for INSERT, UPDATE, DELETE (or just UPDATE as requested)
-- For DELETE, we would use OLD."Id"
DROP TRIGGER IF EXISTS tr_user_changes ON "Users";

CREATE TRIGGER tr_user_changes
AFTER INSERT OR UPDATE OR DELETE
ON "Users"
FOR EACH ROW
EXECUTE FUNCTION notify_user_changes();
