﻿using Microsoft.EntityFrameworkCore;
using MovieLibrary.Data.Contracts;
using MovieLibrary.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MovieLibrary.Data
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieLibraryContext _context;

        public MovieRepository(MovieLibraryContext movieLibraryContext)
        {
            _context = movieLibraryContext ??
                throw new ArgumentNullException(nameof(movieLibraryContext));
        }

        public void DeleteMovie(Movie movie)
        {
            _context.Movies.Remove(movie);
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _context.Movies
                .Include(m => m.MovieCategories)
                .ThenInclude(m => m.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(Expression<Func<Movie, bool>> predicate)
        {
            return await _context.Movies
                .Include(m => m.MovieCategories)
                .ThenInclude(m => m.Category)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task InsertMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
        }

        public void UpdateMovie(Movie movie)
        {
            _context.Entry(movie).State = EntityState.Modified;
        }
    }
}
