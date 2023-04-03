export const UserName = ({ user }) => {
  if (!user) {
    return null;
  }

  return (
    <>
      <img alt="user" className="user-image rounded" src={user.picture} />{" "}
      <span>{user.username}</span>
    </>
  );
};
