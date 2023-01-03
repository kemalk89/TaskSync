import { useEffect, useState } from "react";

export const Tasks = () => {
  const [tasks, setTasks] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const res = await fetch("api/task");
      const json = await res.json();
      setTasks(json);
    };

    fetchData();
  }, []);

  return (
    <div>
      <h1>Tasks</h1>
      <button>Create Task</button>
      <ul>
        {tasks.map((t) => (
          <li key={`t-${t.id}`}>
            #{t.id} - {t.title}
          </li>
        ))}
      </ul>
    </div>
  );
};
