// import axios from 'axios';
// import NextAuth from 'next-auth';
// import CredentialsProvider  from 'next-auth/providers/credentials';

// export default NextAuth({
//     // Custom login & logout pages
//     // pages:

//     // Providers
//     providers: [
//         CredentialsProvider({
//             name: 'Login',

//             credentials: {
//                 username: {label: 'username', type: 'text', placeholder: 'test@test.com'},
//                 password: {label: 'password', type: 'password'}
//             },

//             async authorize(credentials, req) {
//                 const dummyUser = {id: 1, name: "dummy", email: "dummy@dummy.com"}

//                 const formData = new FormData();
//                 formData.append("username", credentials?.username || '');
//                 formData.append('password', credentials?.username || '');

//                 const { data } = await axios({
//                     method: 'post',
//                     url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/sign-up`,
//                     data: formData,
//                 })

//                 console.log(data);

//                 if(dummyUser) return dummyUser;
//                 else return null;
//             },
//         }),

//         CredentialsProvider({
//             name: 'Sign Up',

//             async authorize(credentials, req) {

//             },
//         })
//     ]
// });
