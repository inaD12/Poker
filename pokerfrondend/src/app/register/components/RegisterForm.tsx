"use client";

import { useState } from "react";
import { RegisterUserRequest } from "../../../../users/types/users.types";
import userService from "../../../../users/services/users.services";

const RegisterForm = () => {
    const [apiError, setApiError] = useState<string | null>(null);
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [username, setUsername] = useState<string>("");

    const onSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setApiError(null);

        const data: RegisterUserRequest = { email, password, username};

        try {
            const response = await userService.register(data);
            window.location.href = "/login";
        } catch (error: any) {
            if (error.response?.data?.errors) {
                const errorsObj = error.response.data.errors;
                const allErrors = Object.values(errorsObj)
                    .flat()
                    .join('\n');

                setApiError(allErrors);
            } else {
                const errorMessage =
                    error.response?.data.message || "Something went wrong. Please try again.";
                setApiError(errorMessage);
            }
        }
    };

    return (
        <form
            onSubmit={onSubmit}
            className="flex flex-col gap-4 bg-white w-screen h-screen sm:h-auto sm:min-w-80 sm:max-w-[30%] sm:rounded-xl justify-center text-center text-black p-6 outline-3 outline-blue-700"
        >
            <h1 className="text-4xl mb-6">Register</h1>

            <input
                type="text"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="bg-gray-300 p-2 rounded-md"
            />

            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="bg-gray-300 p-2 rounded-md"
            />

            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="bg-gray-300 p-2 rounded-md"
            />

            {apiError && <div className="text-red-700 text-sm">{apiError}</div>}

            <button
                type="submit"
                className="mx-auto bg-blue-500 text-white p-2 h-[50px] w-[150px] rounded-md hover:bg-blue-600 transition"
            >
                Submit
            </button>
        </form>
    );
};

export default RegisterForm;