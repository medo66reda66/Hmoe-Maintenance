import React, { useState, useEffect } from 'react';
import api from '../api/axiosConfig';
import { Link } from 'react-router-dom';
import Pagination from '../components/Pagination';

const Home = () => {
  const [categories, setCategories] = useState([]);
  const [selectedServiceId, setSelectedServiceId] = useState(null);
  const [companies, setCompanies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Filters and Pagination
  const [categoryPage, setCategoryPage] = useState(1);
  const [categoryTotalPages, setCategoryTotalPages] = useState(1);

  const [companyPage, setCompanyPage] = useState(1);
  const [companyTotalPages, setCompanyTotalPages] = useState(1);
  
  // Filter states
  const [companySearch, setCompanySearch] = useState('');
  const [companyCity, setCompanyCity] = useState('');

  useEffect(() => {
    fetchCategories(categoryPage);
  }, [categoryPage]);

  useEffect(() => {
    if (selectedServiceId) {
      fetchCompanies(selectedServiceId, companyPage);
    }
  }, [selectedServiceId, companyPage, companySearch, companyCity]);

  const fetchCategories = async (page) => {
    setLoading(true);
    try {
      const response = await api.get(`/CompanyProfileAndDetailsService/GetAllServiceCategories?page=${page}`);
      // Assuming response format contains Datarequest and Pagination info
      setCategories(response.data.datarequest || response.data.Data || response.data || []);
      setCategoryTotalPages(response.data.totalPages || 1);
    } catch (err) {
      console.error(err);
      setError('Failed to fetch categories');
    } finally {
      setLoading(false);
    }
  };

  const fetchCompanies = async (serviceId, page) => {
    setLoading(true);
    try {
      const params = new URLSearchParams();
      params.append('page', page);
      if (companySearch) params.append('SearchText', companySearch);
      if (companyCity) params.append('City', companyCity);

      const response = await api.get(`/CompanyProfileAndDetailsService/GetAllCompanyProfileAndDetailsService/${serviceId}?${params.toString()}`);
      setCompanies(response.data.datarequest || []);
      setCompanyTotalPages(response.data.totalPages || 1);
    } catch (err) {
      console.error(err);
      if(err.response?.status === 404) {
         setCompanies([]);
      } else {
         setError('Failed to fetch companies for this service');
      }
    } finally {
      setLoading(false);
    }
  };

  const handleCategoryClick = (serviceId) => {
    setSelectedServiceId(serviceId);
    setCompanyPage(1);
    setCompanies([]);
  };

  return (
    <div className="container mx-auto">
      <h1 className="text-3xl font-bold mb-6">Our Services</h1>
      
      {error && <div className="bg-red-100 text-red-700 p-3 rounded mb-4">{error}</div>}

      <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-4 mb-8">
        {categories.map((category) => (
          <div 
            key={category.id || category.serviceCategoryId}
            onClick={() => handleCategoryClick(category.id || category.serviceCategoryId)}
            className={`p-6 rounded shadow cursor-pointer transition ${selectedServiceId === (category.id || category.serviceCategoryId) ? 'bg-blue-600 text-white' : 'bg-white hover:bg-blue-50 text-gray-800'}`}
          >
            <h3 className="text-xl font-semibold text-center">{category.name || category.serviceName}</h3>
            {category.description && <p className="text-sm mt-2 text-center opacity-80">{category.description}</p>}
          </div>
        ))}
      </div>

      {categories.length > 0 && (
         <Pagination currentPage={categoryPage} totalPages={categoryTotalPages} onPageChange={setCategoryPage} />
      )}

      {selectedServiceId && (
        <div className="mt-12">
          <h2 className="text-2xl font-bold mb-4">Companies Offering this Service</h2>
          
          <div className="flex space-x-4 mb-6">
            <input 
              type="text" 
              placeholder="Search Company..." 
              className="border p-2 rounded"
              value={companySearch}
              onChange={(e) => setCompanySearch(e.target.value)}
            />
            <input 
              type="text" 
              placeholder="Filter by City..." 
              className="border p-2 rounded"
              value={companyCity}
              onChange={(e) => setCompanyCity(e.target.value)}
            />
          </div>

          {loading ? (
             <p>Loading companies...</p>
          ) : companies.length > 0 ? (
            <>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {companies.map((company) => (
                  <div key={company.companyId} className="bg-white p-6 rounded shadow-lg flex flex-col">
                    <h3 className="text-xl font-bold mb-2">{company.companyName}</h3>
                    <p className="text-gray-600 mb-1"><strong>City:</strong> {company.city}</p>
                    <p className="text-gray-600 mb-1"><strong>Address:</strong> {company.address}</p>
                    {company.cost && <p className="text-gray-600 mb-4"><strong>Cost:</strong> ${company.cost}</p>}
                    
                    <Link to={`/company/${company.companyId}`} className="mt-auto bg-blue-600 text-white text-center py-2 rounded hover:bg-blue-700">
                      View Profile & Request Service
                    </Link>
                  </div>
                ))}
              </div>
              <Pagination currentPage={companyPage} totalPages={companyTotalPages} onPageChange={setCompanyPage} />
            </>
          ) : (
            <p className="text-gray-500 italic">No companies found for this service.</p>
          )}
        </div>
      )}
    </div>
  );
};

export default Home;
