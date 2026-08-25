import { useEffect, useState } from "react";
import "./App.css";

const COLLEGE_API = "https://localhost:7108/api/College";
const DEPARTMENT_API = "https://localhost:7108/api/Department";
const COURSE_API = "https://localhost:7108/api/Course";

function App() {
  // =========================
  // ADMIN LOGIN
  // =========================
  const [isLoggedIn, setIsLoggedIn] = useState(
    localStorage.getItem("collegeERPAdmin") === "true"
  );

  const [loginForm, setLoginForm] = useState({
    username: "",
    password: "",
  });

  const [loginError, setLoginError] = useState("");

  const handleLoginChange = (e) => {
    setLoginForm({
      ...loginForm,
      [e.target.name]: e.target.value,
    });
  };

  const handleLogin = (e) => {
    e.preventDefault();

    if (
      loginForm.username === "admin" &&
      loginForm.password === "admin123"
    ) {
      localStorage.setItem("collegeERPAdmin", "true");
      setIsLoggedIn(true);
      setLoginError("");
    } else {
      setLoginError("Invalid username or password.");
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("collegeERPAdmin");
    setIsLoggedIn(false);

    setLoginForm({
      username: "",
      password: "",
    });
  };

  // =========================
  // STATES
  // =========================
  const [colleges, setColleges] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [courses, setCourses] = useState([]);

  const [collegeSearch, setCollegeSearch] = useState("");
  const [departmentSearch, setDepartmentSearch] = useState("");
  const [courseSearch, setCourseSearch] = useState("");

  const [collegeError, setCollegeError] = useState("");
  const [departmentError, setDepartmentError] = useState("");
  const [courseError, setCourseError] = useState("");

  const [collegeForm, setCollegeForm] = useState({
    name: "",
    age: "",
    city: "",
    department: "",
  });

  const [departmentForm, setDepartmentForm] = useState({
    departmentName: "",
    hod: "",
  });

  const [courseForm, setCourseForm] = useState({
    courseName: "",
    departmentId: "",
    duration: "",
  });

  const [editingRollNo, setEditingRollNo] = useState(null);
  const [editingDepartmentId, setEditingDepartmentId] = useState(null);
  const [editingCourseId, setEditingCourseId] = useState(null);

  // =========================
  // LOAD STUDENTS
  // =========================
  const loadColleges = async () => {
    try {
      setCollegeError("");

      const response = await fetch(COLLEGE_API);

      if (!response.ok) {
        throw new Error("Failed to fetch student data");
      }

      const result = await response.json();

      setColleges(result.data || []);
    } catch (error) {
      setCollegeError(error.message);
    }
  };

  // =========================
  // LOAD DEPARTMENTS
  // =========================
  const loadDepartments = async () => {
    try {
      setDepartmentError("");

      const response = await fetch(DEPARTMENT_API);

      if (!response.ok) {
        throw new Error("Failed to fetch department data");
      }

      const result = await response.json();

      if (Array.isArray(result)) {
        setDepartments(result);
      } else if (Array.isArray(result.data)) {
        setDepartments(result.data);
      } else {
        setDepartments([]);
      }
    } catch (error) {
      setDepartmentError(error.message);
    }
  };

  // =========================
  // LOAD COURSES
  // =========================
  const loadCourses = async () => {
    try {
      setCourseError("");

      const response = await fetch(COURSE_API);

      if (!response.ok) {
        throw new Error("Failed to fetch course data");
      }

      const result = await response.json();

      if (Array.isArray(result)) {
        setCourses(result);
      } else if (Array.isArray(result.data)) {
        setCourses(result.data);
      } else {
        setCourses([]);
        throw new Error("Course data format is incorrect");
      }
    } catch (error) {
      console.error("Course Error:", error);
      setCourseError(error.message);
      setCourses([]);
    }
  };

  // =========================
  // LOAD EVERYTHING
  // =========================
  useEffect(() => {
    if (isLoggedIn) {
      loadColleges();
      loadDepartments();
      loadCourses();
    }
  }, [isLoggedIn]);

  // =========================
  // STUDENT
  // =========================
  const handleCollegeChange = (e) => {
    setCollegeForm({
      ...collegeForm,
      [e.target.name]: e.target.value,
    });
  };

  const handleCollegeSubmit = async (e) => {
    e.preventDefault();

    try {
      setCollegeError("");

      const data = {
        name: collegeForm.name,
        age: Number(collegeForm.age),
        city: collegeForm.city,
        department: collegeForm.department,
      };

      let response;

      if (editingRollNo === null) {
        response = await fetch(COLLEGE_API, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data),
        });
      } else {
        response = await fetch(`${COLLEGE_API}/${editingRollNo}`, {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data),
        });
      }

      if (!response.ok) {
        throw new Error("Student operation failed");
      }

      alert(
        editingRollNo === null
          ? "Student added successfully!"
          : "Student updated successfully!"
      );

      setCollegeForm({
        name: "",
        age: "",
        city: "",
        department: "",
      });

      setEditingRollNo(null);

      await loadColleges();
    } catch (error) {
      setCollegeError(error.message);
    }
  };

  const handleEditCollege = (college) => {
    setEditingRollNo(college.rollNo);

    setCollegeForm({
      name: college.name,
      age: college.age,
      city: college.city,
      department: college.department,
    });

    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  };

  const handleDeleteCollege = async (rollNo) => {
    if (!window.confirm("Are you sure you want to delete this student?")) {
      return;
    }

    try {
      setCollegeError("");

      const response = await fetch(`${COLLEGE_API}/${rollNo}`, {
        method: "DELETE",
      });

      if (!response.ok) {
        throw new Error("Student delete failed");
      }

      alert("Student deleted successfully!");

      await loadColleges();
    } catch (error) {
      setCollegeError(error.message);
    }
  };

  const handleCancelCollege = () => {
    setEditingRollNo(null);

    setCollegeForm({
      name: "",
      age: "",
      city: "",
      department: "",
    });
  };

  // =========================
  // DEPARTMENT
  // =========================
  const handleDepartmentChange = (e) => {
    setDepartmentForm({
      ...departmentForm,
      [e.target.name]: e.target.value,
    });
  };

  const handleDepartmentSubmit = async (e) => {
    e.preventDefault();

    try {
      setDepartmentError("");

      const data = {
        departmentName: departmentForm.departmentName,
        hod: departmentForm.hod,
      };

      let response;

      if (editingDepartmentId === null) {
        response = await fetch(DEPARTMENT_API, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data),
        });
      } else {
        response = await fetch(
          `${DEPARTMENT_API}/${editingDepartmentId}`,
          {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
          }
        );
      }

      if (!response.ok) {
        throw new Error("Department operation failed");
      }

      alert(
        editingDepartmentId === null
          ? "Department added successfully!"
          : "Department updated successfully!"
      );

      setDepartmentForm({
        departmentName: "",
        hod: "",
      });

      setEditingDepartmentId(null);

      await loadDepartments();
    } catch (error) {
      setDepartmentError(error.message);
    }
  };

  const handleEditDepartment = (department) => {
    setEditingDepartmentId(department.departmentId);

    setDepartmentForm({
      departmentName: department.departmentName,
      hod: department.hod || "",
    });

    window.scrollTo({
      top: document.body.scrollHeight,
      behavior: "smooth",
    });
  };

  const handleDeleteDepartment = async (departmentId) => {
    if (!window.confirm("Are you sure you want to delete this department?")) {
      return;
    }

    try {
      setDepartmentError("");

      const response = await fetch(
        `${DEPARTMENT_API}/${departmentId}`,
        {
          method: "DELETE",
        }
      );

      if (!response.ok) {
        throw new Error("Department delete failed");
      }

      alert("Department deleted successfully!");

      await loadDepartments();
    } catch (error) {
      setDepartmentError(error.message);
    }
  };

  const handleCancelDepartment = () => {
    setEditingDepartmentId(null);

    setDepartmentForm({
      departmentName: "",
      hod: "",
    });
  };

  // =========================
  // COURSE
  // =========================
  const handleCourseChange = (e) => {
    setCourseForm({
      ...courseForm,
      [e.target.name]: e.target.value,
    });
  };

  const handleCourseSubmit = async (e) => {
    e.preventDefault();

    try {
      setCourseError("");

      const data = {
        courseName: courseForm.courseName,
        departmentId: Number(courseForm.departmentId),
        duration: courseForm.duration,
      };

      let response;

      if (editingCourseId === null) {
        response = await fetch(COURSE_API, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data),
        });
      } else {
        response = await fetch(`${COURSE_API}/${editingCourseId}`, {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data),
        });
      }

      if (!response.ok) {
        throw new Error("Course operation failed");
      }

      alert(
        editingCourseId === null
          ? "Course added successfully!"
          : "Course updated successfully!"
      );

      setCourseForm({
        courseName: "",
        departmentId: "",
        duration: "",
      });

      setEditingCourseId(null);

      await loadCourses();
    } catch (error) {
      setCourseError(error.message);
    }
  };

  const handleEditCourse = (course) => {
    setEditingCourseId(course.courseId);

    setCourseForm({
      courseName: course.courseName,
      departmentId: String(course.departmentId),
      duration: course.duration || "",
    });

    window.scrollTo({
      top: document.body.scrollHeight,
      behavior: "smooth",
    });
  };

  const handleDeleteCourse = async (courseId) => {
    if (!window.confirm("Are you sure you want to delete this course?")) {
      return;
    }

    try {
      setCourseError("");

      const response = await fetch(`${COURSE_API}/${courseId}`, {
        method: "DELETE",
      });

      if (!response.ok) {
        throw new Error("Course delete failed");
      }

      alert("Course deleted successfully!");

      await loadCourses();
    } catch (error) {
      setCourseError(error.message);
    }
  };

  const handleCancelCourse = () => {
    setEditingCourseId(null);

    setCourseForm({
      courseName: "",
      departmentId: "",
      duration: "",
    });
  };

  // =========================
  // SEARCH
  // =========================
  const filteredColleges = colleges.filter((college) =>
    `${college.name} ${college.city} ${college.department}`
      .toLowerCase()
      .includes(collegeSearch.toLowerCase())
  );

  const filteredDepartments = departments.filter((department) =>
    `${department.departmentName} ${department.hod || ""}`
      .toLowerCase()
      .includes(departmentSearch.toLowerCase())
  );

  const filteredCourses = courses.filter((course) =>
    `${course.courseName} ${course.duration}`
      .toLowerCase()
      .includes(courseSearch.toLowerCase())
  );

  // =========================
  // LOGIN PAGE
  // =========================
  if (!isLoggedIn) {
    return (
      <div className="login-page">
        <div className="login-card">
          <div className="login-icon">🎓</div>

          <h1>College ERP</h1>

          <p className="login-subtitle">
            Admin Management Portal
          </p>

          <form onSubmit={handleLogin}>
            <div className="login-field">
              <label>Username</label>

              <input
                type="text"
                name="username"
                placeholder="Enter username"
                value={loginForm.username}
                onChange={handleLoginChange}
                required
              />
            </div>

            <div className="login-field">
              <label>Password</label>

              <input
                type="password"
                name="password"
                placeholder="Enter password"
                value={loginForm.password}
                onChange={handleLoginChange}
                required
              />
            </div>

            {loginError && (
              <p className="login-error">
                {loginError}
              </p>
            )}

            <button type="submit" className="login-button">
              Login
            </button>
          </form>

          <div className="login-footer">
            <span>College ERP Management System</span>
            <span>React + ASP.NET Core + SQL Server</span>
          </div>
        </div>
      </div>
    );
  }

  // =========================
  // DASHBOARD COUNTS
  // =========================
  const totalStudents = colleges.length;
  const totalDepartments = departments.length;
  const totalCourses = courses.length;

  const totalCities = new Set(
    colleges.map((college) => college.city)
  ).size;

  // =========================
  // DASHBOARD
  // =========================
  return (
    <div className="app-wrapper">

      <header className="top-navbar">
        <div>
          <h2>College ERP</h2>
          <span>Administration Portal</span>
        </div>

        <button
          className="logout-button"
          onClick={handleLogout}
        >
          Logout
        </button>
      </header>

      <main className="container">

        <header className="header">
          <div>
            <h1>College ERP Dashboard</h1>
            <p>
              Student, Department and Course Management System
            </p>
          </div>
        </header>

        {/* DASHBOARD CARDS */}
        <div className="dashboard-cards">

          <div className="dashboard-card">
            <div className="card-icon">👨‍🎓</div>
            <h3>Total Students</h3>
            <h2>{totalStudents}</h2>
          </div>

          <div className="dashboard-card">
            <div className="card-icon">🏢</div>
            <h3>Total Departments</h3>
            <h2>{totalDepartments}</h2>
          </div>

          <div className="dashboard-card">
            <div className="card-icon">📚</div>
            <h3>Total Courses</h3>
            <h2>{totalCourses}</h2>
          </div>

          <div className="dashboard-card">
            <div className="card-icon">📍</div>
            <h3>Total Cities</h3>
            <h2>{totalCities}</h2>
          </div>

        </div>

        {/* STUDENT MANAGEMENT */}
        <section className="section">

          <div className="section-header">
            <div>
              <h2>Student Management</h2>
              <p>Manage student records and information</p>
            </div>
          </div>

          <form
            onSubmit={handleCollegeSubmit}
            className="management-form"
          >

            <h3>
              {editingRollNo === null
                ? "Add Student"
                : "Edit Student"}
            </h3>

            <div className="form-row">

              <input
                type="text"
                name="name"
                placeholder="Name"
                value={collegeForm.name}
                onChange={handleCollegeChange}
                required
              />

              <input
                type="number"
                name="age"
                placeholder="Age"
                value={collegeForm.age}
                onChange={handleCollegeChange}
                min="1"
                required
              />

              <input
                type="text"
                name="city"
                placeholder="City"
                value={collegeForm.city}
                onChange={handleCollegeChange}
                required
              />

              <input
                type="text"
                name="department"
                placeholder="Department"
                value={collegeForm.department}
                onChange={handleCollegeChange}
                required
              />

              <button type="submit" className="add-button">
                {editingRollNo === null
                  ? "Add Student"
                  : "Update Student"}
              </button>

              {editingRollNo !== null && (
                <button
                  type="button"
                  className="cancel-button"
                  onClick={handleCancelCollege}
                >
                  Cancel
                </button>
              )}

            </div>
          </form>

          <input
            className="search-box"
            type="text"
            placeholder="Search student by name, city or department..."
            value={collegeSearch}
            onChange={(e) =>
              setCollegeSearch(e.target.value)
            }
          />

          {collegeError && (
            <p className="error">{collegeError}</p>
          )}

          <div className="table-container">

            <table>

              <thead>
                <tr>
                  <th>Roll No</th>
                  <th>Name</th>
                  <th>Age</th>
                  <th>City</th>
                  <th>Department</th>
                  <th>Action</th>
                </tr>
              </thead>

              <tbody>

                {filteredColleges.length > 0 ? (
                  filteredColleges.map((college) => (

                    <tr key={college.rollNo}>

                      <td>{college.rollNo}</td>
                      <td>{college.name}</td>
                      <td>{college.age}</td>
                      <td>{college.city}</td>
                      <td>{college.department}</td>

                      <td className="action-buttons">

                        <button
                          className="edit-button"
                          onClick={() =>
                            handleEditCollege(college)
                          }
                        >
                          Edit
                        </button>

                        <button
                          className="delete-button"
                          onClick={() =>
                            handleDeleteCollege(
                              college.rollNo
                            )
                          }
                        >
                          Delete
                        </button>

                      </td>

                    </tr>

                  ))
                ) : (
                  <tr>
                    <td colSpan="6" className="no-data">
                      No students found
                    </td>
                  </tr>
                )}

              </tbody>

            </table>

          </div>

        </section>

        {/* DEPARTMENT MANAGEMENT */}
        <section className="section">

          <div className="section-header">
            <div>
              <h2>Department Management</h2>
              <p>Manage academic departments and HOD information</p>
            </div>
          </div>

          <form
            onSubmit={handleDepartmentSubmit}
            className="management-form"
          >

            <h3>
              {editingDepartmentId === null
                ? "Add Department"
                : "Edit Department"}
            </h3>

            <div className="form-row">

              <input
                type="text"
                name="departmentName"
                placeholder="Department Name"
                value={departmentForm.departmentName}
                onChange={handleDepartmentChange}
                required
              />

              <input
                type="text"
                name="hod"
                placeholder="Head of Department"
                value={departmentForm.hod}
                onChange={handleDepartmentChange}
              />

              <button type="submit" className="add-button">
                {editingDepartmentId === null
                  ? "Add Department"
                  : "Update Department"}
              </button>

              {editingDepartmentId !== null && (
                <button
                  type="button"
                  className="cancel-button"
                  onClick={handleCancelDepartment}
                >
                  Cancel
                </button>
              )}

            </div>

          </form>

          <input
            className="search-box"
            type="text"
            placeholder="Search department or HOD..."
            value={departmentSearch}
            onChange={(e) =>
              setDepartmentSearch(e.target.value)
            }
          />

          {departmentError && (
            <p className="error">{departmentError}</p>
          )}

          <div className="table-container">

            <table>

              <thead>
                <tr>
                  <th>Department ID</th>
                  <th>Department Name</th>
                  <th>HOD</th>
                  <th>Action</th>
                </tr>
              </thead>

              <tbody>

                {filteredDepartments.length > 0 ? (
                  filteredDepartments.map((department) => (

                    <tr key={department.departmentId}>

                      <td>{department.departmentId}</td>
                      <td>{department.departmentName}</td>
                      <td>
                        {department.hod || "Not Assigned"}
                      </td>

                      <td className="action-buttons">

                        <button
                          className="edit-button"
                          onClick={() =>
                            handleEditDepartment(
                              department
                            )
                          }
                        >
                          Edit
                        </button>

                        <button
                          className="delete-button"
                          onClick={() =>
                            handleDeleteDepartment(
                              department.departmentId
                            )
                          }
                        >
                          Delete
                        </button>

                      </td>

                    </tr>

                  ))
                ) : (
                  <tr>
                    <td colSpan="4" className="no-data">
                      No departments found
                    </td>
                  </tr>
                )}

              </tbody>

            </table>

          </div>

        </section>

        {/* COURSE MANAGEMENT */}
        <section className="section">

          <div className="section-header">
            <div>
              <h2>Course Management</h2>
              <p>Manage courses, departments and course duration</p>
            </div>
          </div>

          <form
            onSubmit={handleCourseSubmit}
            className="management-form"
          >

            <h3>
              {editingCourseId === null
                ? "Add Course"
                : "Edit Course"}
            </h3>

            <div className="form-row">

              <input
                type="text"
                name="courseName"
                placeholder="Course Name"
                value={courseForm.courseName}
                onChange={handleCourseChange}
                required
              />

              <select
                name="departmentId"
                value={courseForm.departmentId}
                onChange={handleCourseChange}
                required
              >

                <option value="">
                  Select Department
                </option>

                {departments.map((department) => (

                  <option
                    key={department.departmentId}
                    value={department.departmentId}
                  >
                    {department.departmentName}
                  </option>

                ))}

              </select>

              <input
                type="text"
                name="duration"
                placeholder="Duration e.g. 6 Months"
                value={courseForm.duration}
                onChange={handleCourseChange}
                required
              />

              <button type="submit" className="add-button">
                {editingCourseId === null
                  ? "Add Course"
                  : "Update Course"}
              </button>

              {editingCourseId !== null && (
                <button
                  type="button"
                  className="cancel-button"
                  onClick={handleCancelCourse}
                >
                  Cancel
                </button>
              )}

            </div>

          </form>

          <input
            className="search-box"
            type="text"
            placeholder="Search course or duration..."
            value={courseSearch}
            onChange={(e) =>
              setCourseSearch(e.target.value)
            }
          />

          {courseError && (
            <p className="error">{courseError}</p>
          )}

          <div className="table-container">

            <table>

              <thead>
                <tr>
                  <th>Course ID</th>
                  <th>Course Name</th>
                  <th>Department</th>
                  <th>Duration</th>
                  <th>Action</th>
                </tr>
              </thead>

              <tbody>

                {filteredCourses.length > 0 ? (
                  filteredCourses.map((course) => {

                    const department = departments.find(
                      (d) =>
                        Number(d.departmentId) ===
                        Number(course.departmentId)
                    );

                    return (
                      <tr key={course.courseId}>

                        <td>{course.courseId}</td>

                        <td>{course.courseName}</td>

                        <td>
                          {department
                            ? department.departmentName
                            : `Department ${course.departmentId}`}
                        </td>

                        <td>
                          {course.duration || "Not specified"}
                        </td>

                        <td className="action-buttons">

                          <button
                            className="edit-button"
                            onClick={() =>
                              handleEditCourse(course)
                            }
                          >
                            Edit
                          </button>

                          <button
                            className="delete-button"
                            onClick={() =>
                              handleDeleteCourse(
                                course.courseId
                              )
                            }
                          >
                            Delete
                          </button>

                        </td>

                      </tr>
                    );
                  })
                ) : (
                  <tr>
                    <td colSpan="5" className="no-data">
                      No courses found
                    </td>
                  </tr>
                )}

              </tbody>

            </table>

          </div>

        </section>

        <footer>
          <p>
            College ERP | React + ASP.NET Core + SQL Server
          </p>
          <span>Admin Portal</span>
        </footer>

      </main>
    </div>
  );
}

export default App;